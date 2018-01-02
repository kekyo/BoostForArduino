using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyHeaderBuilder
{
    public static class Program
    {
        private static string NormalizePath(this string path)
        {
            var elements = path.Split(Path.DirectorySeparatorChar, '/');
            var result = new List<string>();
            for (var index = 0; index < elements.Length; index++)
            {
                var element = elements[index];
                switch (element)
                {
                    case ".":
                        break;
                    case "..":
                        if (result.Count >= 1)
                        {
                            result.RemoveAt(result.Count - 1);
                        }
                        break;
                    default:
                        result.Add(element);
                        break;
                }
            }

            return string.Join("/", result);
        }

        private static async Task<(string childPath, bool isSystem)[]> ParseIncludeDirectivesAsync(string path)
        {
            using (var fs = File.OpenRead(path))
            {
                var tr = new StreamReader(fs);
                var results = new List<(string, bool)>();
                while (tr.EndOfStream == false)
                {
                    var line = (await tr.ReadLineAsync()).Trim();
                    if (line.StartsWith("#include") == false)
                    {
                        continue;
                    }

                    line = line.Substring(8).Trim();
                    var target = line.Trim('<', '"');
                    var index = target.IndexOfAny(new[]{ '>', '"' });
                    if (index >= 0)
                    {
                        target = target.Substring(0, index);
                    }

                    results.Add((target, line.StartsWith("<")));
                }

                return results.ToArray();
            }
        }

        private static async Task WriteIncludeAsync(string fileName, string includePath)
        {
            while (true)
            {
                try
                {
                    using (var fs = File.Create(fileName, 4096, FileOptions.Asynchronous))
                    {
                        var tw = new StreamWriter(fs);
                        await tw.WriteLineAsync($"#include <{includePath}>");
                        await tw.FlushAsync();
                    }
                    break;
                }
                catch (IOException)
                {
                    Console.WriteLine($"Retrying: {fileName}");
                }
            }
        }

        private sealed class Header
        {
            public readonly string Path;
            private readonly HashSet<Header> parents = new HashSet<Header>();

            public bool ReferFromParent => parents.Count >= 1;

            public Header(string path)
            {
                this.Path = path;
            }

            public void AddParent(Header parent)
            {
                lock (parents)
                {
                    parents.Add(parent);
                }
            }
        }

        private static async Task<int> MainAsync(string[] args)
        {
            try
            {
                var srcPath = args[0];
                Directory.SetCurrentDirectory(srcPath);

                //////////////////////////////////////////////////
                
                Console.Write("Cleanup...");

                await Task.WhenAll(
                    Directory.EnumerateFiles(
                        ".",
                        "*.hpp",
                        SearchOption.TopDirectoryOnly)
                    .Select(path => Task.Run(() => File.Delete(path))));

                Console.WriteLine();

                //////////////////////////////////////////////////
                
                Console.Write("Phase 1...");

                var headers =
                    Directory.EnumerateFiles(
                        "boost",
                        "*.hpp",
                        SearchOption.AllDirectories)
                    .Select(path => path.NormalizePath())
                    .ToArray();

                Console.WriteLine();

                //////////////////////////////////////////////////
                
                Console.Write("Phase 2...");

                await Task.WhenAll(
                    headers
                    .Where(path => !path.Contains("/impl/") && !path.Contains("/detail/"))
                    .Select(path => WriteIncludeAsync(path.Replace('/', '_'), path)));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                return ex.HResult;
            }

            return 0;
        }

        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("usage: build <src path>");
                return 0;
            }

            return MainAsync(args).Result;
        }
    }
}

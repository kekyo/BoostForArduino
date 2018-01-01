cd src/boost
for file in *.hpp
do
    echo "#include <boost/$file>" > ../$file
done

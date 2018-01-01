# boost library for Arduino

## What's this?

* Imported boost library at version 1.66.0
  * http://www.boost.org/users/history/version_1_66_0.html
* Unchanged from all original files.
* Prebuild libraries not included. (Yes, lack some codes, ex: boost.asio)

## How to use

### Installation

* Clone from GitHub: (You can use your favorite git client)

```
cd ~/Documents/Arduino/libraries/
git clone https://github.com/kekyo/BoostForArduino
```

* And you can append your compiler toolchain option (At platform.txt) enabling c++11 specification then better results.
* Example: Seeed WioLTE board (On Arduino IDE for Windows)
  * Located: `$HOME\AppData\Local\Arduino15\packages\Seeeduino\hardware\Seeed_STM32F4\1.1.2\platform.txt`

Original line:

```
compiler.cpp.flags=-c -g -Os -w -MMD -ffunction-sections -fdata-sections -nostdlib --param max-inline-insns-single=500 -fno-rtti -fno-exceptions -DBOARD_{build.variant} -D{build.vect} -DERROR_LED_PORT={build.error_led_port} -DERROR_LED_PIN={build.error_led_pin}
```

To insert '-std=gnu++11' option:

```
compiler.cpp.flags=-c -g -Os -std=gnu++11 -w -MMD -ffunction-sections -fdata-sections -nostdlib --param max-inline-insns-single=500 -fno-rtti -fno-exceptions -DBOARD_{build.variant} -D{build.vect} -DERROR_LED_PORT={build.error_led_port} -DERROR_LED_PIN={build.error_led_pin}
```

### Ready: Reference from your sketch

* You can reference boost headers from your sketch.
* You have to trim the "boost" base path for standard boost referrer:

```
// Trim boost base path.
//#include <boost/function.hpp>
#include <function.hpp>
```

Enjoy!!

## License

* Boost license, see [LICENSE](LICENSE_1_0.txt) file.

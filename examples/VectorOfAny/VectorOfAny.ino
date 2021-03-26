
#include <Arduino.h>
#include <list>
#include <boost_any.hpp>

// Copyright 2021 Mia Metzler. All Rights Reserved.
// Licensed under MIT.

/*
;platformio.ini for this lib
;compiling this code may need a lot of time on your cpu
[env:ttgo-lora32-v1]
platform = espressif32
board = ttgo-lora32-v1
framework = arduino
build_flags = -fexceptions -std=gnu++11 -frtti
;-frtti is needed if you want to use thinks like "type()"" in this example
*/

void setup()
{
  Serial.begin(9600);

  std::vector<boost::any> some_values;
  some_values.push_back(10); //Push int (10) into vector
  some_values.push_back("Hello world!"); //Push String into vector
  some_values.push_back(std::string("Your awesome!")); //Push std::string into vector
  
  uint16_t test = 22;
  some_values.push_back(test); //Push uint16_t into vector

  boost::any a = some_values.back(); //Get last element of vector
  
  //Determine its type
  if(a.type() == typeid(uint8_t)){
    Serial.println("Its a uint8!");
  }else if(a.type() == typeid(uint16_t)){
    Serial.println("Its a uint16!");
  }else if(a.type() == typeid(uint32_t)){
    Serial.println("Its a uint32!");
  }else if(a.type() == typeid(uint64_t)){
    Serial.println("Its a uint64!");
  }else if(a.type() == typeid(int8_t)){
    Serial.println("Its a int8!");
  }else if(a.type() == typeid(int16_t)){
    Serial.println("Its a int16!");
  }else if(a.type() == typeid(int32_t)){
    Serial.println("Its a int32!");
  } else if(a.type() == typeid(int64_t)){
    Serial.println("Its a int64!");
  }else if(a.type() == typeid(float)){
    Serial.println("Its a float!");
  } else {
    Serial.println("Unsupported data type! Or at least not expected");
  }
}

void loop()
{
  // put your main code here, to run repeatedly:
}

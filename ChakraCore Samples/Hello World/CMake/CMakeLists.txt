cmake_minimum_required(VERSION 3.2)
project (HelloWorld)

# set(ICU_INCLUDE_PATH ......) # -> if you have a custom ICU setup
# set(CHAKRACORE_PATH "................/Github/ChakraCore/") # -> location of the ChakraCore Repo
# set(CHAKRACORE_LIB_TYPE ....Release or Test or Debug) # -> by default Release
if (NOT CHAKRACORE_PATH)
    message (FATAL_ERROR "You should set CHAKRACORE_PATH to run this sample.
            You may set it from CMake or command line (-DCHAKRACORE_PATH=<path>)
            ")
endif()

message ("-- Compiling ChakraCore..")

# Embed ChakraCore
include(ChakraCoreEmbed.cmake)

add_executable (Sample sample.cpp)

target_include_directories (Sample
  PUBLIC ${CMAKE_CURRENT_SOURCE_DIR}
  ${CHAKRACORE_INCLUDE_PATH} # add ChakraCore include path
  )

target_link_libraries (Sample
  ${CHAKRACORE_LINKER_OPTIONS} # add ChakraCore linker options
  )

cmake_minimum_required(VERSION 3.2)

project(ChakraCoreSamples)

find_package(PkgConfig)

pkg_check_modules(PC_ICU_UC icu-uc)
pkg_check_modules(PC_ICU_I18N icu-i18n)

if(EXISTS "${CMAKE_SOURCE_DIR}/ChakraCore/CMakeLists.txt")
	SET(CC_AUTODETECT_CPU ON CACHE BOOL "" FORCE)
	SET(CC_AUTODETECT_ICU ON CACHE BOOL "" FORCE)
	add_subdirectory(ChakraCore)
else()
	# TODO: Detect ChakraCore when installed as a System Package.
	message(FATAL_ERROR "ChakraCore not found")
endif()

if(NOT TARGET Chakra.Pal OR
	NOT TARGET Chakra.Common.Core OR
	NOT TARGET Chakra.Jsrt )
	message(FATAL_ERROR "ChakraCore not found")
endif()


add_custom_target(CopyChakraHeaders ALL
	COMMAND ${CMAKE_COMMAND} -E make_directory "${CMAKE_BINARY_DIR}/include"
	COMMAND ${CMAKE_COMMAND} -E copy_if_different
		"${CMAKE_SOURCE_DIR}/ChakraCore/lib/Jsrt/ChakraCore.h"
		"${CMAKE_BINARY_DIR}/include"
	COMMAND ${CMAKE_COMMAND} -E copy_if_different
		"${CMAKE_SOURCE_DIR}/ChakraCore/lib/Jsrt/ChakraCommon.h"
		"${CMAKE_BINARY_DIR}/include"
	COMMAND ${CMAKE_COMMAND} -E copy_if_different
		"${CMAKE_SOURCE_DIR}/ChakraCore/lib/Jsrt/ChakraCommonWindows.h"
		"${CMAKE_BINARY_DIR}/include"
	COMMAND ${CMAKE_COMMAND} -E copy_if_different
		"${CMAKE_SOURCE_DIR}/ChakraCore/lib/Jsrt/ChakraDebug.h"
		"${CMAKE_BINARY_DIR}/include"
)

add_subdirectory("ChakraCore Samples/Hello World/Linux_OSX")


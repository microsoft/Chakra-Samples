cmake_minimum_required(VERSION 3.2)

project(ChakraCoreSamples)

find_package(PkgConfig)

pkg_check_modules(PC_ICU_UC icu-uc)
pkg_check_modules(PC_ICU_I18N icu-i18n)

if(PC_ICU_UC_FOUND AND PC_ICU_I18N_FOUND)
	#target_link_libraries(myexe ${PC_ICU_UC_LIBRARIES} ${PC_ICU_I18N_LIBRARIES})
endif()
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

add_subdirectory("ChakraCore Samples/Hello World/Linux_OSX")

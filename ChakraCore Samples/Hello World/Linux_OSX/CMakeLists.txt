
add_executable (ChakraHelloWorld sample.cpp)

if(CMAKE_SYSTEM_NAME STREQUAL Linux)
  set(LINKER_START_GROUP -Wl,--start-group)
  set(LINKER_END_GROUP -Wl,--end-group)
endif()

if(CC_TARGETS_X86)
  set(lib_target "-m32")
endif()

# common link deps
set(lib_target "${lib_target}"
   -Wl,-undefined,error
  ${LINKER_START_GROUP}
  ChakraCore
  Chakra.Common.Core
  Chakra.Jsrt
  Chakra.Pal

  

  ${LINKER_END_GROUP}
  )



SET_TARGET_PROPERTIES(ChakraHelloWorld
                               PROPERTIES COMPILE_FLAGS "-std=c++11")

target_link_libraries (ChakraHelloWorld ${lib_target})
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


add_dependencies(ChakraHelloWorld CopyChakraHeaders)


if(PC_ICU_UC_FOUND AND PC_ICU_I18N_FOUND)
	target_link_libraries(ChakraHelloWorld ${PC_ICU_UC_LIBRARIES} ${PC_ICU_I18N_LIBRARIES})
endif()

target_include_directories (ChakraHelloWorld
  PUBLIC "${CMAKE_BINARY_DIR}/include")

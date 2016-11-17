
add_executable (ChakraOpenGLEngine
	OpenGLEngine/Canvas.cpp
	OpenGLEngine/Task.cpp
	OpenGLEngine/main.cpp
	OpenGLEngine/ChakraCoreHost.cpp
	OpenGLEngine/Shape.cpp
)

if(CMAKE_SYSTEM_NAME STREQUAL Linux)
  set(LINKER_START_GROUP -Wl,--start-group)
  set(LINKER_END_GROUP -Wl,--end-group)
endif()

if(CC_TARGETS_X86)
  set(lib_target "-m32")
endif()


# Link against OpenGL
target_include_directories (ChakraOpenGLEngine  PUBLIC "${OPENGL_INCLUDE_DIR}")

# Link against GLEW
if(PC_GLEW_FOUND)
	# This should include internationalization.
	target_include_directories (ChakraOpenGLEngine PUBLIC "${PC_GLEW_INCLUDE_DIRS}" )
endif()

# Link against GLFW.
if(PC_GLFW_FOUND)
	# This should include internationalization.
	target_include_directories (ChakraOpenGLEngine PUBLIC "${PC_GLFW_INCLUDE_DIRS}")
endif()

# common link deps
if(TARGET ChakraCore)
	#Use ChakraCore shared Library.
	set(lib_target "${lib_target}"
	  -Wl,-undefined,error
      ${LINKER_START_GROUP}
	  ChakraCore
	  ${LINKER_END_GROUP}
	  ${PC_GLEW_LDFLAGS}
	  ${PC_GLFW_LDFLAGS}
	  ${OPENGL_gl_LIBRARY}
	  ${PC_ICU_UC_LIBRARIES}
	  ${PC_ICU_I18N_LIBRARIES}
	  )
else()
	#Use ChakraCore Static Library.
	set(lib_target "${lib_target}"
	   -Wl,-undefined,error
      ${LINKER_START_GROUP}
	  Chakra.Common.Core
	  Chakra.Jsrt
	  Chakra.Pal
	  ${LINKER_END_GROUP}
	  ${PC_GLEW_LDFLAGS}
	  ${PC_GLFW_LDFLAGS}
	  ${OPENGL_gl_LIBRARY}
      ${LINKER_END_GROUP}
	  )
endif()



target_link_libraries(ChakraOpenGLEngine ${lib_target} )

#use C++11
SET_TARGET_PROPERTIES(ChakraOpenGLEngine PROPERTIES COMPILE_FLAGS
	 "-std=c++11 -Wno-pragma-once-outside-header"
	)

add_dependencies(ChakraOpenGLEngine CopyChakraHeaders)
target_include_directories (ChakraOpenGLEngine  PUBLIC "${CMAKE_BINARY_DIR}/include")



if(NOT CC_XCODE_PROJECT)
  set(CC_LIB_EXT "so")
  # Post build step to copy the built shared library
  # to BuildLinux (or whatever the CMakeBuildDir is)
  if(CMAKE_SYSTEM_NAME STREQUAL Darwin)
    set(CC_LIB_EXT "dylib")
  endif()
  if(TARGET ChakraCore)
    add_custom_command(TARGET ChakraOpenGLEngine POST_BUILD
      COMMAND ${CMAKE_COMMAND} -E copy_if_different
        "${CMAKE_BINARY_DIR}/ChakraCore/libChakraCore.${CC_LIB_EXT}"
        "${CMAKE_BINARY_DIR}/"
    )
  endif()
  # Copy  ChakraOpenGLEngine to Binary Directory.
  add_custom_command(TARGET ChakraOpenGLEngine POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_if_different
	"${CMAKE_BINARY_DIR}/ChakraCore Samples/OpenGL Engine/ChakraOpenGLEngine"
	"${CMAKE_BINARY_DIR}/"
  )
  # Copy the Example Javascript file.
  add_custom_command(TARGET ChakraOpenGLEngine POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_if_different
	"${CMAKE_CURRENT_SOURCE_DIR}/OpenGLEngine/app.js"
	"${CMAKE_BINARY_DIR}/"
	)

endif()


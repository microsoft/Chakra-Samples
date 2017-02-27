#-------------------------------------------------------------------------------------------------------
# Copyright (C) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
#-------------------------------------------------------------------------------------------------------

if (NOT ICU_INCLUDE_PATH)
    if (CMAKE_SYSTEM_NAME STREQUAL Darwin)
        set (ICU_INCLUDE_PATH "/usr/local/opt/icu4c/include")
        set (ICU_INCLUDE_PATH_ARG "--icu=${ICU_INCLUDE_PATH}")
    endif()
else()
    set (ICU_INCLUDE_PATH_ARG "--icu=${ICU_INCLUDE_PATH}")
endif()

if (NOT CHAKRACORE_LIB_TYPE)
    set (CHAKRACORE_LIB_TYPE "Release")
    set (CHAKRACORE_WIN_SHORT "x64_release")
    set (COREE_LIB_TYPE "")
elseif (CHAKRACORE_LIB_TYPE STREQUAL "Test")
    set (CHAKRACORE_LIB_TYPE "Test")
    set (COREE_LIB_TYPE "--test-build")
elseif (CHAKRACORE_LIB_TYPE STREQUAL "Release")
    set (CHAKRACORE_LIB_TYPE "Release")
    set (CHAKRACORE_WIN_SHORT "x64_release")
    set (COREE_LIB_TYPE "")
elseif (CHAKRACORE_LIB_TYPE STREQUAL "Debug")
    set (CHAKRACORE_LIB_TYPE "Debug")
    set (COREE_LIB_TYPE "--debug")
    set (CHAKRACORE_WIN_SHORT "x64_debug")
endif()

if (CMAKE_SYSTEM_NAME STREQUAL Windows)
    set (COREE_SCRIPT_EXEC_COMMAND "msbuild")
    # todo: allow platform selection ?
    set (COREE_SCRIPT_ARGS "/m" "/p:Configuration=${CHAKRACORE_LIB_TYPE}" "/p:Platform=x64" "Build\\Chakra.Core.sln")
else()
    set (COREE_SCRIPT_EXEC_COMMAND "./build.sh")
    set (COREE_SCRIPT_ARGS ${ICU_INCLUDE_PATH_ARG} ${COREE_LIB_TYPE} "-j=2")
endif()

execute_process(COMMAND ${COREE_SCRIPT_EXEC_COMMAND} ${COREE_SCRIPT_ARGS}
    WORKING_DIRECTORY ${CHAKRACORE_PATH}
    RESULT_VARIABLE COREE_CMD_EXIT_CODE
    OUTPUT_VARIABLE COREE_EXEC_OUTPUT
    )

if (COREE_CMD_EXIT_CODE)
    message(FATAL_ERROR "ChakraCore build has failed with error: ${COREE_CMD_EXIT_CODE} ${COREE_EXEC_OUTPUT}")
endif()

if (NOT CMAKE_SYSTEM_NAME STREQUAL Windows)
    set (CHAKRACORE_BUILD_PATH "${CHAKRACORE_PATH}/out/${CHAKRACORE_LIB_TYPE}/lib/")
    find_library(CHAKRACORE_STATIC_LIB_PATH ChakraCoreStatic PATHS ${CHAKRACORE_BUILD_PATH} NO_DEFAULT_PATH)
    set (CHAKRACORE_INCLUDE_PATH
        "${CHAKRACORE_PATH}/lib/Jsrt/")
else() # Windows
    set (CHAKRACORE_BUILD_PATH "${CHAKRACORE_PATH}\\Build\\VcBuild\\bin\\${CHAKRACORE_WIN_SHORT}")
    find_library(CHAKRACORE_STATIC_LIB_PATH ChakraCore PATHS ${CHAKRACORE_BUILD_PATH} NO_DEFAULT_PATH)
    set (CHAKRACORE_LINKER_OPTIONS ${CHAKRACORE_STATIC_LIB_PATH})
    set (CHAKRACORE_INCLUDE_PATH
        "${CHAKRACORE_PATH}\\lib\\Jsrt\\")
endif()
message ("ChakraCore Library is available at ${CHAKRACORE_STATIC_LIB_PATH}")

if (CMAKE_SYSTEM_NAME STREQUAL Darwin)
    if (ICU_INCLUDE_PATH)
        set(ICU_CC_PATH "${ICU_INCLUDE_PATH}/../lib/")
        find_library(ICUUC icuuc PATHS ${ICU_CC_PATH} NO_DEFAULT_PATH)
        find_library(ICU18 icui18n PATHS ${ICU_CC_PATH} NO_DEFAULT_PATH)
        find_library(ICUDATA icudata PATHS ${ICU_CC_PATH} NO_DEFAULT_PATH)
        if(ICUUC)
            set(ICULIB
              ${ICUUC}
              ${ICU18}
              ${ICUDATA}
              )
        endif()
    endif()
    set (CHAKRACORE_LINKER_OPTIONS ${CHAKRACORE_STATIC_LIB_PATH}
      "-framework CoreFoundation"
      "-framework Security"
      "${ICULIB}")
elseif (CMAKE_SYSTEM_NAME STREQUAL Linux)
    set (CHAKRACORE_LINKER_OPTIONS
      ${CHAKRACORE_STATIC_LIB_PATH}
      icuuc
      pthread
      dl)
endif()

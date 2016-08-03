#-------------------------------------------------------------------------------------------------------
# Copyright (C) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#-------------------------------------------------------------------------------------------------------

IDIR=../lib/Jsrt
CC=g++

LIBRARY_PATH=../BuildLinux/$(BUILD_TYPE)/lib
LDIR=$(LIBRARY_PATH)/../pal/src/libChakra.Pal.a \
	$(LIBRARY_PATH)/Common/Core/libChakra.Common.Core.a \
	$(LIBRARY_PATH)/Jsrt/libChakra.Jsrt.a

ifeq (darwin, ${PLATFORM})
ICU4C_LIBRARY_PATH ?= /usr/local/opt/icu4c
CFLAGS=-lstdc++ -std=c++11 -I$(IDIR)
FORCE_STARTS=-Wl,-force_load,
FORCE_ENDS=
LIBS=-framework CoreFoundation -framework Security -lm -ldl -Wno-c++11-compat-deprecated-writable-strings \
	-Wno-deprecated-declarations -Wno-unknown-warning-option -o sample.o
LDIR=$(LDIR)\
	$(ICU4C_LIBRARY_PATH)/lib/libicudata.a \
	$(ICU4C_LIBRARY_PATH)/lib/libicuuc.a \
	$(ICU4C_LIBRARY_PATH)/lib/libicui18n.a
else
CFLAGS=-lstdc++ -std=c++0x -I$(IDIR)
FORCE_STARTS=-Wl,--whole-archive
FORCE_ENDS=-Wl,--no-whole-archive
LIBS=-pthread -lm -ldl -licuuc -lunwind-x86_64 -Wno-c++11-compat-deprecated-writable-strings \
	-Wno-deprecated-declarations -Wno-unknown-warning-option -o sample.o
endif

testmake:
	$(CC) sample.cpp $(CFLAGS) $(FORCE_STARTS) $(LDIR) $(FORCE_ENDS) $(LIBS)

.PHONY: clean

clean:
	rm sample.o

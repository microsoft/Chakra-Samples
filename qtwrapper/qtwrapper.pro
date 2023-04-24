QT += core
QT -= gui

CONFIG += c++17 -rdynamic console
unix {
    QMAKE_CXXFLAGS += -std=c++17 -rdynamic
}
TARGET = wrappersample
CONFIG -= app_bundle

TEMPLATE = app

SOURCES += main.cpp \
    qtwrapper/qjsengine.cpp \
    qtobject.cpp

DEFINES += QT_DEPRECATED_WARNINGS


INCLUDEPATH += /home/melidrissi/.root/Applications/ChakraCore/lib/Jsrt/
HEADERS += \
    qtwrapper/qjsengine.h \
    qtobject.h

unix {
    LIBS += -L/home/melidrissi/.root/Applications/ChakraCore/out/Release -lChakraCore -pthread -lm -ldl -licuuc
}
win32 {
    LIBS += 'C:/PATH_TO_LIBRARY/ChakraCore.Lib'
    INCLUDEPATH += PATH_TO_LIBRARY_INCLUDES/include
}

DISTFILES += \
    README.md

# Project layout

```
root/
	|-- OpenGLEngine/						// source folder
		|-- dep/ 							// dependencies	
			|-- ChakraCore/ 				// pieces from ChakraCore engine
			|-- glew-1.13.0/				// glew library
			|-- glfw-3.1.2/					// glfw library
		|-- app.js 							// sample bouncing ball application built with the engine
		|-- Canvas.h/cpp					// opengl canvas
		|-- ChakraCoreHost.h/cpp			// JavaScript host and bindings to native methods
		|-- main.cpp						// main program
		|-- Shape.h/cpp						// opengl shapes
		|-- Task.h/cpp						// a JavaScript task in the message queue
	|-- CustomAPI.md 						// Documentation for custom APIs
	|-- Layout.md 							// project layout
	|-- LICENSE								// project license
	|-- OpenGLEngine.sln					// project solution file
	|-- README.md 							// README
```

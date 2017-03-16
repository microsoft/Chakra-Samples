#pragma once
#include <stdio.h>
#include <stdlib.h>
#include "Canvas.h"

Canvas::Canvas() 
{
	// initilizae glew
	glewExperimental = GL_TRUE;
	glewInit();

	// initilizae glfw and window
	if (!glfwInit()) {
		fprintf(stderr, "ERROR: could not start GLFW3\n");
		return;
	}

	// create and set up window
	window = glfwCreateWindow(640, 480, "App", NULL, NULL);
	if (!window)
	{
		glfwTerminate();
		exit(EXIT_FAILURE);
	}
	glfwMakeContextCurrent(window);
	glfwSwapInterval(1);
}

void Canvas::setMouseClickCallback(GLFWmousebuttonfun func) 
{
	glfwSetMouseButtonCallback(window, func);
}

void Canvas::addShape(GLShape* shape) 
{
	_shapes.push_back(shape);
}

void Canvas::removeShape(GLShape* shape) 
{
	for (int i = 0; i<_shapes.size(); ++i) {
		if (_shapes[i] == shape) {
			_shapes.erase(_shapes.begin() + i);
			break;
		}
	}
}

void Canvas::render() 
{
	// part of this method from glfw documentation - http://www.glfw.org/docs/latest/quick.html
	if (!glfwWindowShouldClose(window))
	{
		float ratio;
		int width, height;
		glfwGetFramebufferSize(window, &width, &height);
		ratio = width / (float)height;
		glViewport(0, 0, width, height);
		glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT);
		glMatrixMode(GL_PROJECTION);
		glLoadIdentity();
		glOrtho(-ratio, ratio, -1.f, 1.f, 1.f, -1.f);
		glMatrixMode(GL_MODELVIEW);
		glLoadIdentity();

		// buffer all shapes on heap
		for (std::vector<GLShape*>::iterator it = _shapes.begin(); it != _shapes.end(); ++it) {
			(*it)->render();
		}

		glfwSwapBuffers(window);
		glfwPollEvents();
	}
}

Canvas::~Canvas() 
{
	glfwTerminate();
}

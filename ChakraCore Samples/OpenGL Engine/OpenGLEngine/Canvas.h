#pragma once
#include "Shape.h"
#include "GL/glew.h"
#include "GLFW/glfw3.h"
#include <vector>

using namespace std;

// opengl canvas
class Canvas
{
private:
	GLFWwindow* window;										// opengl window
	vector<GLShape*> _shapes;								// shapes added to canvas
public:
	Canvas();
	void setMouseClickCallback(GLFWmousebuttonfun func);	// set a mouse event callback on canvas
	void addShape(GLShape* shape);							// add a  to canvas
	void removeShape(GLShape* shape);						// remove a GLShape to canvas
	void render();											// paint a frame
	~Canvas();												// terminate drawing session
};

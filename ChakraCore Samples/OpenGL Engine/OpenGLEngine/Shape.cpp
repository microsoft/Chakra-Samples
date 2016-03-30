#include "Shape.h"
#include "GL/glew.h"
#include "GLFW/glfw3.h"

using namespace std;

// all lines/triangles/quads/polygons are considered GLPolygon
GLenum GLPolygonShape(int numPoints) 
{
	switch (numPoints)
	{
		case 1:
			return GL_POINTS;
		case 2:
			return GL_LINES;
		case 3:
			return GL_TRIANGLES;
		case 4:
			return GL_QUADS;
		default:
			return GL_POLYGON;
	} 
}

GLTriple::GLTriple() 
{
	_x = 0.0f;
	_y = 0.0f;
	_z = 0.0f;
};

GLTriple::GLTriple(float x, float y, float z) 
{
	_x = x;
	_y = y;
	_z = z;
};

void GLShape::setColor(GLTriple color) 
{
	_color = color;
};

void GLShape::rotate(float rotateAngle, GLTriple rotateAxis) 
{
	_rotateAngle = rotateAngle;
	_rotateAxis = rotateAxis;
};

GLPoint::GLPoint() 
{
	_x = 0;
	_y = 0;
	_z = 0;
};

GLPoint::GLPoint(float x, float y, float z) 
{
	_x = x;
	_y = y;
	_z = z;
};

void GLPoint::render() 
{
	glRotatef(_rotateAngle, _rotateAxis._x, _rotateAxis._y, _rotateAxis._z);
	glColor3f(_color._x, _color._y, _color._z);
	glBegin(GL_POINTS);
	glVertex3f(_x, _y, _z);
	glEnd();
};

GLPolygon::GLPolygon(vector<GLPoint> points) 
{
	_points = points;
};

void GLPolygon::setPosition(vector<GLPoint> points) 
{
	_points = points;
}
void GLPolygon::render() 
{
	glRotatef(_rotateAngle, _rotateAxis._x, _rotateAxis._y, _rotateAxis._z);
	glColor3f(_color._x, _color._y, _color._z);
	glBegin(GLPolygonShape(_points.size()));
	for (vector<GLPoint>::iterator it = _points.begin(); it != _points.end(); ++it) {
		glVertex3f(it->_x,it->_y, it->_z);
	}
	glEnd();
};

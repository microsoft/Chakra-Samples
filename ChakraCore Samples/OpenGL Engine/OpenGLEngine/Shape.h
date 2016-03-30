#pragma once
#include <vector>

using namespace std;

// class having 3 data points - used to represent color and rotation axis
class GLTriple 
{
public:
	float _x, _y, _z;
	GLTriple();
	GLTriple(float x, float y, float z);
};

// base class OpenGL shape that can be added to canvas
class GLShape 
{
public: 
	GLTriple _color = GLTriple(1.0f, 1.0f, 1.0f);
	float _rotateAngle = 0.0f;
	GLTriple _rotateAxis = GLTriple(0.0f, 0.0f, 0.0f);
	void setColor(GLTriple color);
	void rotate(float rotateAngle, GLTriple rotateAxis);
	virtual void render() = 0;
};

// opengl point 
class GLPoint : public GLShape 
{
public:
	float _x, _y, _z;
	GLPoint();
	GLPoint(float x, float y, float z);
	void render();
};

// opengl polygons, incl. GL_LINES, GL_TRIANGLES, GL_QUADS, GL_POLYGON
class GLPolygon : public GLShape 
{
public:
	vector<GLPoint> _points;
	GLPolygon(vector<GLPoint> points);
	void setPosition(vector<GLPoint> points);
	void render();
};

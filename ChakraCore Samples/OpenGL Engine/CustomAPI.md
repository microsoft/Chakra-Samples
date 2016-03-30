# Custom API Documentation 

This file documents the custom APIs available in JavaScript. See example of usage in [app.js](OpenGLEngine/app.js). 

```
// ************************************************************
//				         General methods 
// ************************************************************

/**
 * Print the expression.
 *
 * @param {string} expr The expression to be printed
 */ 
console.log(expr);

/**
 * Calls a function after a specified delay.
 * This is slightly different than setTimeout in browsers.
 *
 * @param {Function} func The function to be called.
 * @param {number} delay Delay time in milliseconds.
 */ 
setTimeout(func, delay);

/**
 * Repeatedly calls a function with a fixed time delay between each call.
 * This is slightly different than setInternal in browsers.
 *
 * @param {Function} func The function to be called.
 * @param {number} delay Delay time in milliseconds.
 */ 
setInternal(func, delay);

// ************************************************************
//    				    Shape constructors 
// ************************************************************

/**
 * Create a new point.
 *
 * @constructor
 * @param {number} x X coordinate of the point.
 * @param {number} y Y coordinate of the point.
 * @param {number} z Z coordinate of the point.
 * @return {Point} The new Point object.
 */
Point(x, y, z);

/**
 * Create a new Line.
 *
 * @constructor
 * @param {Point} point1 One Point of the Line.
 * @param {Point} point2 One Point of the Line.
 * @return {Line} The new Line object.
 */
Line(point1, point2);

/**
 * Create a new Triangle.
 *
 * @constructor
 * @param {Point} point1 One Point of the Triangle.
 * @param {Point} point2 One Point of the Triangle.
 * @param {Point} point3 One Point of the Triangle.
 * @return {Triangle} The new Triangle object.
 */
Triangle(point1, point2, point3);

/**
 * Create a new Quad.
 *
 * @constructor
 * @param {Point} point1 One Point of the Quad.
 * @param {Point} point2 One Point of the Quad.
 * @param {Point} point3 One Point of the Quad.
 * @param {Point} point4 One Point of the Quad.
 * @return {Quad} The new Quad object.
 */
Quad(point1, point2, point3, point4);

/**
 * Create a new Polygon. Can take any non-zero number of points.
 *
 * @constructor
 * @param {[Points]} [points] Array of Points for the Polygon.
 * @return {Polygon} The new Polygon object.
 */
Polygon([points]);

// ************************************************************
//				   Common methods for shapes 
// ************************************************************

/**
 * Set the color of a shape with RGB value.
 *
 * @param {number} rotateAngle The angle to rotate.
 * @param {number} x coordinate of rotation axis.
 * @param {number} y coordinate of rotation axis.
 * @param {number} z coordinate of rotation axis.
 */
[Point/Line/Triangle/Quad/Polygon].prototype.rotate(rotateAngle, x, y ,z);

/**
 * Set the color of a shape with RGB value.
 *
 * @param {number} R value of color; should be scaled within [0, 1].
 * @param {number} G value of color; should be scaled within [0, 1].
 * @param {number} B value of color; should be scaled within [0, 1].
 */
[Point/Line/Triangle/Quad/Polygon].prototype.setColor(R, G, B);

/**
 * Set the position of a shape. Cannot be called on a Point.
 *
 * @param {[Points]} [points] Array of Points for the new position. The required number of points is based on the shape.
 */
[Line/Triangle/Quad/Polygon].prototype.setPosition([points]);


// ************************************************************
//				         Canvas methods 
// ************************************************************

/**
 * Add a shape to canvas.
 *
 * @param {Shape} The shape that will be added to canvas.
 */
canvas.addShape(shape);

/**
 * Remove a shape to canvas.
 *
 * @param {Shape} The shape that will be remove to canvas.
 */
canvas.removeShape(shape);

/**
 * Set callback to a mouse click event on canvas.
 *
 * @param {Function} callback The callback to be called when mouse click is fired. The callback takes a object pos,
 *                   which has two properties x and y - the coordinates of the click on canvas.
 */
canvas.setMouseClickCallback(callback);

/**
 * Render a frame of all shapes added to canvas.
 */
canvas.render();
```

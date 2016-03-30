// Sample - Bouncing Balls
// Click on the window to drop a ball

'use strict';

// constants
let ballRadius = 0.1,
    ballColor = [Math.random(), Math.random(), Math.random()],
    acceleration = -0.005,
    collisionVelocityLose = 0.1,
    groundLevel = -0.9,
    epislon = 0.0002,
    delta = 0.1;

// util function to set shape color
function setShapeColor(shape, colorArray) {
    shape.setColor(colorArray[0], colorArray[1], colorArray[2]);
}

class Ball {
    constructor(center, radius, color) {
        this.center = center;
        this.radius = radius;
        this.color = color;
        this.velocity = 0; 
        this.timestamp = Date.now();
        this.circle = this.createCircle(this.center, this.radius, this.color);      // the ball
        canvas.addShape(this.circle);
    }

    // create a circle - a polygon with a lot of vertices in OpenGL
    createCircle(center, radius, color) {
        this.points = [];
        for (let theta = 0; theta < 2 * Math.PI; theta += delta) {
            var point = new Point(center.x + radius * Math.cos(theta), center.y + radius * Math.sin(theta), center.z);
            this.points.push(point);
        }
        let circle = new Polygon(this.points);
        setShapeColor(circle, color);
        return circle;
    }

    // update the circle's current position and velocity
    update() {
        // update only if the ball has not stopped on the ground
        if ((Math.abs(this.center.y - this.radius - groundLevel) > epislon) || (Math.abs(this.velocity > epislon))) {
            // remove the old circle
            canvas.removeShape(this.circle);
            // approximate the circle's current position and velocity at this time
            let newTimestamp = Date.now();
            let timeElapsed = newTimestamp - this.timestamp;
            this.velocity += timeElapsed / 1000 * acceleration;
            this.center.y += this.velocity * timeElapsed;
            // handle collision with ground
            if (this.center.y < this.radius + groundLevel) {
                this.center.y = this.radius + groundLevel;
                this.velocity = -this.velocity * (1 - collisionVelocityLose);
                this.color = [Math.random(), Math.random(), Math.random()];
            }
            this.circle = this.createCircle(this.center, this.radius, this.color);
            // add the new circle
            canvas.addShape(this.circle);
            this.timestamp = newTimestamp;
        }
    }
}

let balls = [];

// create a dropping ball where the mouse clicks on the canvas
canvas.setMouseClickCallback((pos) => {
    let center = pos;
    center.z = 0.0;
    balls.push(new Ball(center, ballRadius, ballColor));
});

// draw a frame of all balls
function mainloop() {
    for (let ball of balls) {
        ball.update();
    }
    canvas.render();
}

setInterval(mainloop, 0);

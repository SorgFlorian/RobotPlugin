RobotPlugin
===========

This is an empty plugin used for the course animation and virtual reality (computer science) at the Aalen University of Applied Sciences.

The students have to implement an AI for a robot in a 3d classroom and run the plugin successfully.

The simulation is successful, if all stones are inside the target area.

Version
-------

1.0

Usage
-----
* ```App.LogMessage(string msg)``` will show your message in the robot gui for debugging reasons.
* ```App.DrawLine(Vector start, Vector end, float duration)```  will draw the vector in the world for debugging reasons.
* The class ```Vector``` also provides some static methods for vector arithmetics.
* The object ```Robot.Data``` provides the current position and the direction vectors of the robot (normalized).
* The methods ```Robot.GoForward()```, ```Robot.GoBackward()```, ```Robot.TurnLeft()``` and ```Robot.TurnRight()``` will set the behaviour of the robot for the current game tick and must be set every tick if desired.
* The object ```Field.stones``` provides an array of the stone objects that are placed in the world.
* The object ```Field.TargetArea``` provides the position and radius of the target area.
* You should implement your AI inside the  mehtod ```OnTick()```. This Method is called every game tick.
* You may use custom private methods.
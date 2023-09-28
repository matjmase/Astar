# A\* pathfinding algorithm

Optimized version of Djikstra's algorithm used to find paths between two points in a computationally optimized way.

## Legend

<img src="https://github.com/matjmase/Astar/blob/main/Screenshots/Legend.jpg" width="200" />

A. Configure the size of the grid (E).

B. Clear all items in the grid (E).

C. Draw mode. Click to toggle.
D. About popup
E. Drawable grid.
F. Distance coefficient from start to any given point(wraps around curves and obstacles).
G. Distance coefficient from any give point to destination (as the crow flies, no obstacles taken in to account.)
H. Delta Time for the Animate Gradient function (K).
I. Pathing mode. Orthogonal, Orthogonal and Diagonal, Orthogonal and Diagonal with no corner obstructing the diagonal move.
J. Calculates the path from the Start point to the End point.
K. Animates gradient so that you can see how the algorithm works.
L. Displays the distance of the path.

## Experiment

Play around with the settings to get different gradients and traversal characteristics.

Start off by making G a value of 0.

<img src="https://github.com/matjmase/Astar/blob/main/Screenshots/Djikstra.jpg" width="200" />

This should resemble how Djiksta's algorithm would traverse the terraign.

## Make mazes

Watch the algorithm solve the maze real time with the Animate Gradient button K.

<img src="https://github.com/matjmase/Astar/blob/main/Screenshots/FindingPath.jpg" width="200" />

You can also cut to the chase by clicking J to just solve the maze.

<img src="https://github.com/matjmase/Astar/blob/main/Screenshots/PathFound.jpg" width="200" />

## Conclusion

The Algorithm will attempt to solve any maze you give it, but if there is no valid path between the points, you will get and popup exception message.

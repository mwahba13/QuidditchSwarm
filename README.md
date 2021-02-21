# QuidditichSwarm
Assignment for CPSC 565 (Emergent Computing) implementing a Quidditch match using a multi-agent swarm paradigm.

Quidditch is a game for wizards where two teams fly on broomsticks to try and catch a golden snitch while dodging bludgers, quafflers, wafflers, etc.
Who am I kidding? You don't really care about that do you?

This project is a simplified implementation of Quidditch in Unity using a multi-agent swarm paradigm to simulate the two teams.

Each player has has a set number of forces propelling them through the world. One force leads them towards the snitch, another pushes them away from colliding with teammates. The forces can be visualized using the in-game debug menu. The goal of the game is to try and catch the snitch. Each time a member of a team catches a snitch, their team will score a point. If a team successively captures the snitch before the other team scores a point, then that team will gain a bonus point. The game ends when a team reaches a score of 100.

## Instructions
1. Download or clone repository
2. Open in Unity

## Controls
W,A,S,D - Move Camera Forward, Backwards, Left, Right <br />
Q,E, - Rotate Camera <br />
CTRL - Move Camera Down <br />
Space - Move Camera Up <br />

## Team Traits 
The stats of players on both teams are randomly determined based on the values entered at the main menu.

## Ingame Debug Menu






![prism dodger+ logo](Assets/Textures/Icon.png?raw=true "Title")
# prism dodger+ 
### dodge prisms and perfect your best times as you roll your way to the finish in the infinite, procedurally generated levels of ***prism dodger+***

- green prisms move sideways across the track
- red prisms move along the track between walls
- purple prisms rotate around the track

controls:
- wasd/arrowkeys/left thumbstick: move ball
- enter/start: start game
- escape: quit game

game by jaden goter for Rice Games interview project

music by graphiqs groove

## play
***prism dodger+*** can be played in your browser on [its itch.io page](https://picross.itch.io/prism-dodger-plus) or [on windows](Builds/Win/prism%20dodger%2B.zip).

## documentation of work done
### gameplay modifications
- the original prompt for the game specified that the player should navigate through levels of large prisms that move in unsurpising ways. in ***prism dodger+***, the levels contain small prisms, but a small path for the player to navigate. if the prisms were any bigger, they would be too big for the path.
- the original prompt specifies that the obstructions should move vertically or horizontally at different speeds. in ***prism dodger+***, the purple obstruction moves in a circle.
- the original prompt didn't say how many levels there should be. in ***prism dodger+***, there are infinite, procedurally generated levels.

### noteworthy scripts
- [Manager.cs](Assets/Scripts/Game%20State/Manager.cs) is a singleton that controls the game state. it loads levels, controls the difficulty, keeps track of progress, and initializes the obstructions with their serialized data.
- [TimeManager.cs](Assets/Scripts/Game%20State/TimeManager.cs) is a manager that keeps track of and saves all of the player's best times.
- [UIManager.cs](Assets/Scripts/UI/UIManager.cs) is a manager that controls and updates all the UI in the game.
- [LevelGenerator.cs](Assets/Scripts/Level/LevelGenerator.cs) procedurally generates each level. the game is played entirely in one scene (besides the title scene) for optimization.
- [Obstruction.cs](Assets/Scripts/Level/Obstruction.cs) controls the obstructions' movements. it can be given an [ObstructionData.cs](Assets/Scripts/Level/ObstructionData.cs) to change how it behaves.
- [ObstructionData.cs](Assets/Scripts/Level/ObstructionData.cs) is a serializable class that [Manager.cs](Assets/Scripts/Game%20State/Manager.cs) can serialize, deserialize, save, and load into obstructions through [Obstruction.cs](Assets/Scripts/Level/Obstruction.cs).
- [Player.cs](Assets/Scripts/Player/Player.cs) captures input and controls the player.

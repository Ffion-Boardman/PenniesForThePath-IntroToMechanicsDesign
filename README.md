# Pennies for the Path - Introduction to Mechanics Design
![Static Badge](https://img.shields.io/badge/Status%3A-On_Portfolio-green?style=flat-square&link=https%3A%2F%2Fwww.ffionboardman.com%2Fgame-projects%2Fsimple-scene)
![Static Badge](https://img.shields.io/badge/Unity-grey?style=flat-square&logo=unity)
![Static Badge](https://img.shields.io/badge/Version-6000.17.1f-lightgrey?style=flat-square&logo=unity)
![Static Badge](https://img.shields.io/badge/Language-C%23-lightgrey?style=flat-square&logo=csharp)
![Static Badge](https://img.shields.io/badge/Game_Type-2D-lightgrey?style=flat-square)
![Static Badge](https://img.shields.io/badge/Platformer-lightgrey?style=flat-square)
![Static Badge](https://img.shields.io/badge/Adventure-lightgrey?style=flat-square)
![Static Badge](https://img.shields.io/badge/Solo-lightgrey?style=flat-square)

Made for my 'Introduction to Mechanics Design' module during my second year at The University of Staffordshire.  
Check out the [project-overview](https://www.ffionboardman.com/game-projects/pennies-for-the-path) of Pennies for the Path on my portfolio!

## Programming Highlights
### Player Controller
The Player Controller was intentionally designed to avoid using the Update() loop as much as possible, to practice optimization methods in my code.  
Link: [CharacterMovement.cpp](Assets/Scripts/Player/CharacterMovement.cs)
### Dynamic UI
This was my first time using Unity's UI Toolkit instead of the original UI system, with intentions of ensuring the UI is clean and dynamically created when the game is started.  
Link: [HealthUI.cpp](Assets/Scripts/UIHandlers/HealthUI.cs)  
Link: [ScoreUI.cpp](Assets/Scripts/UIHandlers/ScoreUI.cs)  
### Sound System
The Sound-System's code is mainly focused on creating a designer-friendly system for storing and displaying the code within the inspector.    
Link: [SoundManager.cs](Assets/Scripts/Other/SoundManager.cs)

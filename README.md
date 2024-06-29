# C-Sharp Development Final Assignment
## Hexoban - A Console Game

**Hexoban** is a console-based game inspired by the classic **Sokoban** game we did in class build using C# and the .NET framework. Developed as the final project for the C-Sharp Development class, Hexoban brings unique twists to the original game with modified win conditions and NPC interactions.

Table of Contents

- Introduction
- Features
- Installation
- Authors
- License

## Introduction

Hexoban is a puzzle game where players must push boxes (represented by purple runes) to target locations (green runes) to win. The game features multiple levels, NPC interactions, and various control options for a well-rounded gameplay experience.

## Features

- Main Menu:
    - Start/Continue The Game
    - View the tutorial
    - Exit the game.
- Three Levels:
    - increased difficulty with level progression
    - one NPC with dialog per level
- Player Controls:
    - Move with arrow keys.
    - Interact with NPCs using 'E' key.
    - Undo moves with 'Z' key.
    - Restart level with 'R' key.
    - Return to main menu with 'M' key.
    - Save/Load Game: Players can save their game progress through NPC interactions and continue later.
- JSON Integration: Game setup, dialogues, tutorial, and saved states are managed using JSON files.

## Installation
To install and run Hexoban, follow these steps:

1. Clone the repository:
    - `git clone https://github.com/michi-992/CD_FinalAssignment.git`

2. .NET version:
    - The console application was build using `.NET 8`

    - Changing the version: change the target framework in the `game.csproj` & `libs.csproj` files

3. JSON configuration:
    - Create environment variable `GAME_SETUP_PATH` pointing to Setup.json in the project

    - Create environment variable `GAME_SETUP_PATH_SAVED` pointing to  SavedFile.json in the project

    - Create environment variable `GAME_DIALOG_SETUP_PATH` pointing to Dialog.json file in the project

    - Create environment variable `GAME_TUTORIAL_SETUP_PATH` pointing to Tutorial.json in the project

4. Move into the project and then game directory:
    - `cd CD_FinalAssignment\game`

5. **.vscode configuration:**
    - Create a `.vscode` folder in the root directory of the project.
    - Inside the `.vscode` folder, create a `launch.json` file with the following content:
      ```json
        {
        "version": "0.2.0",
        "configurations": [
            {
                "name": ".NET Core Launch (console)",
                "type": "coreclr",
                "request": "launch",
                "preLaunchTask": "build",
                "program": "${workspaceFolder}/game/bin/Debug/net8.0/game.dll",
                "args": [],
                "cwd": "${workspaceFolder}",
                "stopAtEntry": false,
                "console": "integratedTerminal",
                "internalConsoleOptions": "neverOpen"
                },
            ]
        }
      ```
    - Also, create a `tasks.json` file inside the `.vscode` folder with the following content:
      ```json
        {
        "version": "2.0.0",
        "tasks": [
            {
                "label": "build",
                "command": "dotnet",
                "type": "process",
                "args": [
                    "build"
                ],
                "options": {
                    "cwd": "${workspaceFolder}/game"
                },
                "problemMatcher": "$msCompile"
            }
        ]
        }   
      ```

6. Build the project: 
    - `dotnet build`

7. Run the project:
    - `dotnet run`

## Authors
- Michaela Topalovic: [GitHub Profile](https://github.com/michi-992)
- Noëlle Jamöck: [GitHub Profile](https://github.com/seoyangjadu)

The project structure and base code for the game was provided by [lbappel](https://github.com/lbappel/). The in class project state was worked on by two more people [Lukas Gruber](https://github.com/yeetboy02) and [Manuel Prammer](https://github.com/Pramsi). The final assignment portion of this project was solely done by Michaela and Noëlle.


## License
This project is licensed under the MIT License.



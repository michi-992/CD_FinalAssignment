using libs;

using System.Security.Cryptography;
using Newtonsoft.Json;

class Program
{    
    static private int currLevel = 0;

    static private bool MainMenuOpen = true;
    static void Main(string[] args)
    {
        //Setup
        Console.CursorVisible = false;
        var engine = GameEngine.Instance;
        var inputHandler = InputHandler.Instance;
        var tutorialDialog = new TutorialDialog();
        
        engine.Setup(currLevel);
        currLevel = engine.GetCurrentLevel();
        bool savedByNPC = false;

        // Main game loop
        while (true)
        {
            if (MainMenuOpen)
            {
                ShowMainMenu(tutorialDialog);
            } else
            {
                DisplayTip(engine.GetTip());

                engine.Render();

                // CHECK WIN CONDITION
                if (engine.allTargetsFilled()) {
                    nextLevel(engine);
                    break;
                }

                // Check for restart key press
                if (engine.GetRestartGame()) {
                    restartGame(engine);
                    break;
                }

                if (engine.dialog.currentNode.text == "I saved the game for you!" && !savedByNPC) {
                    engine.saveGame();
                    savedByNPC = true;
                }
            
                // Handle keyboard input
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                inputHandler.Handle(keyInfo);
                engine.Update();

                if (keyInfo.Key == ConsoleKey.M)
                {
                    MainMenuOpen = true;
                    continue;
                }
            }
        }
    }

    static private void DisplayTip(string tip)
    {
        Console.SetCursorPosition(0, 1);
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, 1);
        Console.WriteLine(tip);
    }

    static private void nextLevel(GameEngine engine) {
        // remove map history once level completed
        engine.removeHistory();

        // end level or increse current level and set it in game engine
        if (currLevel == 2) endGame();
        currLevel++;
        engine.SetCurrentLevel(currLevel);

        Console.Clear();
        Main(null);
    }

    static private void ShowMainMenu(TutorialDialog tutorialDialog)
    {
        Console.Clear();
        Console.WriteLine("HEXOBAN");
        Console.WriteLine();
        Console.WriteLine("=== Main Menu ===");
        Console.WriteLine("1. Start/Continue Game");
        Console.WriteLine("2. Tutorial");

        int choice;
        while (true)
        {
            Console.Write("Choose an option: ");
            if (int.TryParse(Console.ReadLine(), out choice) && (choice == 1 || choice == 2))
            {
                break;
            }
            Console.WriteLine("Invalid choice, please try again.");
        }

        if (choice == 1)
        {
            MainMenuOpen = false;
            Console.Clear();
        }
        else if (choice == 2)
        {
            Console.Clear();
            tutorialDialog.ShowTutorial();
        }
    }

    static private void endGame() {
        // overwrite saved JSON game state 
        var gameState = new GameState { currentLevel = null, gameObjects =  new List<GameObject>() };
        string output = JsonConvert.SerializeObject(gameState);
        File.WriteAllText("../SavedFile.json", output);

        Console.Clear();
        Console.WriteLine("HEXOBAN");
        Console.WriteLine("Congratulations! You have completed the game!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        Environment.Exit(0);
    }


    static private void restartGame(GameEngine engine) {
        // overwrite saved JSON game state
        var gameState = new GameState { currentLevel = null, gameObjects =  new List<GameObject>() };
        string output = JsonConvert.SerializeObject(gameState);
        File.WriteAllText("../SavedFile.json", output);

        // remove map history, set level to 0, set flag to false
        engine.removeHistory();
        currLevel = 0;
        engine.SetCurrentLevel(currLevel);
        engine.SetRestartGame(false);

        Console.Clear();
        Main(null);
    }
}
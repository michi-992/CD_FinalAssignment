using libs;

using System.Security.Cryptography;
using Newtonsoft.Json;

class Program
{    
    static private int currLevel = 0; // sets current game level

    static private bool MainMenuOpen = true; // flag to indicate if main menu is open
    static void Main(string[] args)
    {
        //Setup
        Console.CursorVisible = false; // hides cursor
        var engine = GameEngine.Instance; // gets GameEngine singleton
        var inputHandler = InputHandler.Instance; //gets InputHandler singleton
        inputHandler.MainMenuRequested += OnMainMenuRequested; // watches for MainMenuRequested event
        var tutorialDialog = new TutorialDialog(); // creates new instance of TutorialDialog
        
        engine.Setup(currLevel); // sets up engine with current level
        currLevel = engine.GetCurrentLevel(); // retrieves current level
        bool savedByNPC = false; // flag to check if game was saved

        // Main game loop
        while (true)
        {
            if (MainMenuOpen)
            {
                ShowMainMenu(tutorialDialog); // displays main menu
            } else
            {
                DisplayTip(engine.GetHelperMethods().GetTip()); // displays game tip

                engine.Render();

                // CHECK WIN CONDITION
                if (engine.GetHelperMethods().allTargetsFilled()) {
                    nextLevel(engine); // proceed to next level
                    break;
                }

                // Check for restart key press
                if (engine.GetMap().GetRestartGame()) {
                    restartGame(engine); // restarts game
                    break;
                }

                // Saves game if NPC saved it
                if (engine.GetDialog().currentNode.text == "I saved the game for you!" && !savedByNPC) {
                    engine.GetHelperMethods().saveGame(); // saves game
                    savedByNPC = true; // sets flag indicating if game was saved to true
                }
            
                // Handle keyboard input
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                inputHandler.Handle(keyInfo); // handles input
                engine.Update(); // updates game state
            }
        }
    }

    // displays tip in the second line
    static private void DisplayTip(string tip)
    {
        Console.SetCursorPosition(0, 1);
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, 1);
        Console.WriteLine(tip);
    }

    // proceeds to next level
    static private void nextLevel(GameEngine engine) {
        // remove map history once level completed
        engine.GetMap().removeHistory();

        // end level or increse current level and set it in game engine
        if (currLevel == 2) endGame();
        currLevel++;
        engine.SetCurrentLevel(currLevel);

        Console.Clear();
        Main(null);
    }

    // Event handler for when main menu is opened
    private static void OnMainMenuRequested(object sender, EventArgs e)
    {
        MainMenuOpen = true;
    }

    // displays main menu
    static private void ShowMainMenu(TutorialDialog tutorialDialog)
    {
        Console.Clear();
        Console.WriteLine("HEXOBAN");
        Console.WriteLine();
        Console.WriteLine("=== Main Menu ===");
        Console.WriteLine("1. Start/Continue Game");
        Console.WriteLine("2. Tutorial");
        Console.WriteLine("3. Exit the game");

        int choice;
        while (true)
        {
            Console.Write("Choose an option: ");
            if (int.TryParse(Console.ReadLine(), out choice) && (choice == 1 || choice == 2 || choice == 3))
            {
                break;
            }
            Console.WriteLine("Invalid choice, please try again.");
        }

        // closes menu
        if (choice == 1)
        {
            MainMenuOpen = false;
            Console.Clear();
        }

        // shows tutorial
        else if (choice == 2)
        {
            Console.Clear();
            tutorialDialog.ShowTutorial();
        }

        // exits out of the game
        else if (choice == 3) 
        {
            Environment.Exit(0);
        }
    }

    // method to end game
    static private void endGame() {
        // overwrite saved JSON game state 
        var gameState = new GameState { currentLevel = null, gameObjects =  new List<GameObject>() };
        string output = JsonConvert.SerializeObject(gameState); // serializes game state to json string
        File.WriteAllText("../SavedFile.json", output); // writes save data to save file

        Console.Clear();
        Console.WriteLine("HEXOBAN");
        Console.WriteLine("");
        Console.WriteLine("Congratulations! You have completed the witch trials!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        Environment.Exit(0); // exits
    }

    // restarts game
    static private void restartGame(GameEngine engine) {
        // overwrite saved JSON game state
        var gameState = new GameState { currentLevel = null, gameObjects =  new List<GameObject>() };
        string output = JsonConvert.SerializeObject(gameState);
        File.WriteAllText("../SavedFile.json", output);

        // remove map history, set level to 0, set flag to false
        engine.GetMap().removeHistory();
        currLevel = 0; // resets current level to first level
        engine.SetCurrentLevel(currLevel);
        engine.GetMap().SetRestartGame(false);

        Console.Clear();
        Main(null);
    }
}
using libs;

using System.Security.Cryptography;
using Newtonsoft.Json;

class Program
{    
    static private int currLevel = 0;
    static void Main(string[] args)
    {
        //Setup
        Console.CursorVisible = false;
        var engine = GameEngine.Instance;
        var inputHandler = InputHandler.Instance;

        var tutorialDialog = new TutorialDialog();
        tutorialDialog.StartTutorial();

        // Continue to game or restart tutorial based on player choice
        while (!tutorialDialog.ContinueToGame)
        {
            tutorialDialog.StartTutorial();
        }

        var npcs = InitializeNPCs();
        engine.SetNPCs(npcs);
        
        engine.Setup(currLevel);
        currLevel = engine.GetCurrentLevel();

        // Main game loop
        while (true)
        {
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
            
            // Handle keyboard input
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            inputHandler.Handle(keyInfo);
            engine.Update();

            // Check for NPC interaction
            CheckForNPCInteraction();

            engine.Update();
        }
    }

    static private void CheckForNPCInteraction()
    {
        var player = Player.Instance;
        foreach (var npc in GameEngine.Instance.GetNPCs())
        {
            player.InteractWithNPC(npc);
        }
    }

    static private List<NPC> InitializeNPCs()
    {    
        DialogNode node1 = new DialogNode("Hello, welcome to Sokoban!");

        node1.AddResponse("Hello!", new DialogNode("What brings you here today?", new List<Response> {
            new Response("I want to save the game", new DialogNode("I'll get right on that!"), () => GameEngine.Instance.saveGame()),
            new Response("I want to reset the game", new DialogNode("Sure thing!"), () => GameEngine.Instance.restartGame()),
            new Response("I want to undo the last action", new DialogNode("Alright!"), () => GameEngine.Instance.revertHistory()),
            new Response("I want to load the saved game", new DialogNode("Here we go!"), () => GameEngine.Instance.loadGame())
        }));

        NPC npc = new NPC(1, 1, node1); // Example position (1, 1)

        return new List<NPC> { npc };
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

    static private void endGame() {
        // overwrite saved JSON game state 
        var gameState = new GameState { currentLevel = null, gameObjects =  new List<GameObject>() };
        string output = JsonConvert.SerializeObject(gameState);
        File.WriteAllText("../SavedFile.json", output);

        Console.Clear();
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
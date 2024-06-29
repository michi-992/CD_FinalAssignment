namespace libs;

// singleton class to handle user input
public sealed class InputHandler{

    private static InputHandler? _instance;

    // reference to the game engine
    private GameEngine engine;

    // time of the last interaction
    private DateTime lastInteractionTime = DateTime.MinValue;
    // cooldown period between interactions
    private readonly TimeSpan interactionCooldown = TimeSpan.FromSeconds(5);

    // event triggered when the main menu is requested
    public event EventHandler MainMenuRequested;

    // property to get the singleton instance of InputHandler
    public static InputHandler Instance {
        get{
            if(_instance == null)
            {
                _instance = new InputHandler();
            }
            return _instance;
        }
    }

    // private constructor to prevent external instantiation
    private InputHandler() {
        //INIT PROPS HERE IF NEEDED
        engine = GameEngine.Instance;
    }

    // method to handle keyboard input
    public void Handle(ConsoleKeyInfo keyInfo)
    {
        GameObject focusedObject = engine.GetFocusedObject();

        if (focusedObject != null) {
            // Handle keyboard input to move the player
            switch (keyInfo.Key)
            {
                // moving the player with arrow keys
                case ConsoleKey.UpArrow:
                    focusedObject.Move(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    focusedObject.Move(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    focusedObject.Move(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    focusedObject.Move(1, 0);
                    break;

                // undoing the last step(s) and restarting the game
                case ConsoleKey.Z:
                    engine.GetMap().resetHistoryMethod();
                    break;
                case ConsoleKey.R:
                    engine.GetMap().restartGame();
                    break;

                // interaction with NPCs
                case ConsoleKey.E:
                    HandleInteraction();
                    break;

                // open main menu
                case ConsoleKey.M:
                    OnMainMenuRequested();
                    break;
                default:
                    break;
            }
        }
        
    }

    private void OnMainMenuRequested()
    {
        MainMenuRequested?.Invoke(this, EventArgs.Empty);
    }

    private void HandleInteraction()
    {
        GameObject focusedObject = engine.GetFocusedObject();

        if (DateTime.Now - lastInteractionTime >= interactionCooldown)
        {
            lastInteractionTime = DateTime.Now;
            if (Player.Instance.NextToNPC(engine.GetMap().GetMap()))
            {
                focusedObject.startDialog();
            }
        }
    }
}
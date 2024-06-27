namespace libs;

public sealed class InputHandler{

    private static InputHandler? _instance;
    private GameEngine engine;

    private DateTime lastInteractionTime = DateTime.MinValue;
    private readonly TimeSpan interactionCooldown = TimeSpan.FromSeconds(5);

    public static InputHandler Instance {
        get{
            if(_instance == null)
            {
                _instance = new InputHandler();
            }
            return _instance;
        }
    }

    private InputHandler() {
        //INIT PROPS HERE IF NEEDED
        engine = GameEngine.Instance;
    }

    public void Handle(ConsoleKeyInfo keyInfo)
    {
        GameObject focusedObject = engine.GetFocusedObject();

        if (focusedObject != null) {
            // Handle keyboard input to move the player
            switch (keyInfo.Key)
            {
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
                case ConsoleKey.Z:
                    engine.revertHistory();
                    break;
                case ConsoleKey.S:
                    engine.saveGame();
                    break;
                case ConsoleKey.R:
                    engine.restartGame();
                    break;
                case ConsoleKey.E:
                    HandleInteraction();
                    break;
                default:
                    break;
            }
        }
        
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
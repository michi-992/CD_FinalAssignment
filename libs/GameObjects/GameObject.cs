namespace libs;

public class GameObject : IGameObject, IMovement
{
    // fields for character represenation, color, and position
    private char _charRepresentation = '#';
    private ConsoleColor _color;

    private int _posX;
    private int _posY;
    
    private int _prevPosX;
    private int _prevPosY;

    // Public property for GameObject type
    public GameObjectType Type;

    // Default constructor initial position and default color    
    public GameObject() {
        this._posX = 5;
        this._posY = 5;
        this._color = ConsoleColor.Gray;
    }

    // Constructor with parameters for custom position
    public GameObject(int posX, int posY){
        this._posX = posX;
        this._posY = posY;
    }

    // Constructor with parameters for custom position and color
    public GameObject(int posX, int posY, ConsoleColor color){
        this._posX = posX;
        this._posY = posY;
        this._color = color;
    }

    // Property for character representation of the GameObject
    public char CharRepresentation
    {
        get { return _charRepresentation ; }
        set { _charRepresentation = value; }
    }

    // Property for color of the GameObject
    public ConsoleColor Color
    {
        get { return _color; }
        set { _color = value; }
    }

    // Property for X coordinate of the GameObject's position
    public int PosX
    {
        get { return _posX; }
        set { _posX = value; }
    }


    // Property for Y coordinate of the GameObject's position
    public int PosY
    {
        get { return _posY; }
        set { _posY = value; }
    }

    // Method to get the previous Y coordinate of the GameObject's position
    public int GetPrevPosY() {
        return _prevPosY;
    }
    
    // Method to get the previous X coordinate of the GameObject's position
    public int GetPrevPosX() {
        return _prevPosX;
    }

    // Nullable dialog instance and list of dialog nodes
    public Dialog? dialog;
    protected List<DialogNode> dialogNodes = new List<DialogNode>();

    // Method to start a dialog asynchronously
    public void startDialog() {
        dialog = GameEngine.Instance.GetDialog(); // Get dialog instance from the game engine
        
        Task dialogTask = Task.Run(() => {
            this.dialog.Start(); // Start the dialog
        });

        dialogTask.Wait();
        dialog = null;
    }

    // Method to move the GameObject by specified increments
    public void Move(int dx, int dy) {
        if (dialog == null) { // Check if a dialog is not currently active
            // Store current x & y position as previous x & y
            _prevPosX = _posX;
            _prevPosY = _posY;
            _posX += dx;
            _posY += dy;
        }
    }

    // Virtual method called on collision with another GameObject (to be overridden)
    virtual public void onCollision(GameObject obj, GameObject?[,] map) {
    }
}

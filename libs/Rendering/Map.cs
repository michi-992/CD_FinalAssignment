namespace libs;
using Newtonsoft.Json;

// represents the game map with different layers for representation and game objects
public class Map {
    // 2D array for the visual representation of the map
    private char[,] RepresentationalLayer;

    // 2D array for storing game objects on the map
    private GameObject?[,] GameObjectLayer;

    // History of game states for undo functionality
    public List<GameObject?[,]> history = new List<GameObject?[,]>();

    // flag to reset history
    public bool resetHistory = false;

    // width and height of the map
    private int _mapWidth;
    private int _mapHeight;

    // default constructor initializing a map of size 30x8
    public Map () {
        _mapWidth = 30;
        _mapHeight = 8;
        RepresentationalLayer = new char[_mapHeight, _mapWidth];
        GameObjectLayer = new GameObject[_mapHeight, _mapWidth];
    }

    // constructor initializing a map with specified width and height
    public Map (int width, int height) {
        _mapWidth = width;
        _mapHeight = height;
        RepresentationalLayer = new char[_mapHeight, _mapWidth];
        GameObjectLayer = new GameObject[_mapHeight, _mapWidth];
    }

    // method to get the current game object layer
    public GameObject?[,] GetMap(){
        return GameObjectLayer;
    }

    // method to initialize the map with default floor objects
    public void Initialize()
    {

            GameObjectLayer = new GameObject[_mapHeight, _mapWidth];
            // Initialize the map with some default values
            for (int i = 0; i < GameObjectLayer.GetLength(0); i++)
            {
                for (int j = 0; j < GameObjectLayer.GetLength(1); j++)
                {
                    GameObjectLayer[i, j] = new Floor();
                }
            }
        RepresentationalLayer = new char[_mapHeight, _mapWidth];
    }

    // property to get and set the width of the map
    public int MapWidth
    {
        get { return _mapWidth; } // Getter
        set { _mapWidth = value; Initialize();} // Setter
    }

    // property to get and set the height of the map
    public int MapHeight
    {
        get { return _mapHeight; } // Getter
        set { _mapHeight = value; Initialize();} // Setter
    }

    // method to get the game object that is at a specific position
    public GameObject Get(int x, int y){
        return GameObjectLayer[x, y];
    }

    // method to save the current state to history
    public void SaveToHistory() {
        if (resetHistory) {
            resetHistory = false;
        }
        else if (history.Count == 0 || (history[history.Count - 1] != GameObjectLayer)) {
            history.Add(GameObjectLayer.Clone() as GameObject?[,]);
        }
    }

    // method to revert to the previous state from history
    public void revertHistory() {
        if (history.Count > 1) {
            history.RemoveAt(history.Count - 1);
            GameObjectLayer = history[history.Count - 1];
        };
    }

    // method to clear the history
    public void clearHistory() {
        history = new List<GameObject?[,]>();
    }

    // method to set a game object at its new position and update its previous position
    public void Set(ref GameObject gameObject){
        int posY = gameObject.PosY;
        int posX = gameObject.PosX;
        int prevPosY = gameObject.GetPrevPosY();
        int prevPosX = gameObject.GetPrevPosX();
        
        // clear the previous position if within bounds
        if (prevPosX >= 0 && prevPosX < _mapWidth &&
                prevPosY >= 0 && prevPosY < _mapHeight)
        {
            if (GameObjectLayer[prevPosY, prevPosX] is Floor) {
                GameObjectLayer[prevPosY, prevPosX] = new Floor();
            }
        }

        // set the new position if within bounds
        if (posX >= 0 && posX < _mapWidth &&
                posY >= 0 && posY < _mapHeight)
        {
            GameObjectLayer[gameObject.PosY, gameObject.PosX] = gameObject;
            RepresentationalLayer[gameObject.PosY, gameObject.PosX] = gameObject.CharRepresentation;
        }
    }
}
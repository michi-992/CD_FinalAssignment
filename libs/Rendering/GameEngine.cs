using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace libs;

using System.Security.Cryptography;
using Newtonsoft.Json;

// Singleton class representing the game engine
public sealed class GameEngine
{
    private static GameEngine? _instance;
    private IGameObjectFactory gameObjectFactory;

    // property to get the singleton instance of the GameEngine
    public static GameEngine Instance {
        get {
            if (_instance == null) {
                _instance = new GameEngine();
            }
            return _instance;
        }
    }

    private GameEngine() {
        // Initialize properties here if needed
        gameObjectFactory = new GameObjectFactory();
    }


    // currently focused game object (if any) and getter
    private GameObject? _focusedObject;
    public GameObject GetFocusedObject() {
        return _focusedObject;
    }


    //  game map and getter
    private Map map = new Map();
    public Map GetMap() {
        return map;
    }


    // list of all game objects and getter
    private List<GameObject> gameObjects = new List<GameObject>();
    public List<GameObject> GetGameObjects() {
        return gameObjects;
    }


    // helper methods instance and getter
    private HelperMethods helperMethods = new HelperMethods();
    public HelperMethods GetHelperMethods() {
        return helperMethods;
    }

    // initial and current game level as well as getter and setter for current game level
    private int? initalGameLevel = 0; // initial level - gets changed if there is a saved game
    private int currentGameLevel = 0; // tracks game level and changes once nextLevel is called in Program
    public int GetCurrentLevel() {
        return currentGameLevel;
    }
    public void SetCurrentLevel(int value) {
        currentGameLevel = value;
    }


    // dialog for the current level ang getter
    private static Dialog? dialog  = null;
    public Dialog? GetDialog() {
        return dialog;
    }


    // setup method to initialize the game
    public void Setup(int currLevel = 0) {
        // added for proper display of game characters
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // read game data, saved game data, and dialog data from files
        dynamic gameData = FileHandler.ReadJson();
        dynamic gameDataSaved = FileHandler.ReadSavedJson();
        dynamic dialogData = FileHandler.ReadDialogJson();

        // checks whether there is a currently saved game and whether it is not bigger than the current level
        initalGameLevel = gameDataSaved.currentLevel; 
        if (initalGameLevel != null && initalGameLevel !> currLevel) SetCurrentLevel(initalGameLevel.Value);
        
        // checks whether setup gameObjects or saved game gameObjects should be used
        // the helper functions overwrites the gameobject properties for movable game objects
        var gameObjectsJSON = gameData[currentGameLevel].gameObjects;
        if (gameDataSaved.gameObjects.Count > 0 && (initalGameLevel !> currLevel || initalGameLevel == currLevel)) {
            gameObjectsJSON = helperMethods.UseSavedObjects(gameObjectsJSON, gameDataSaved);
        }

        // initialize the dialog for the current level
        var currDialog = dialogData[currentGameLevel].dialog;
        dialog = helperMethods.createDialog(currDialog);

        // initialize the map dimensions
        map.MapWidth = gameData[currentGameLevel].map.width;
        map.MapHeight = gameData[currentGameLevel].map.height;

        // initialize game objects list and add each game object from JSON to the list
        gameObjects = new List<GameObject>();
        foreach (var gameObject in gameObjectsJSON) {
            AddGameObject(CreateGameObject(gameObject));
        }
        
        // set the focused object to the first player object found
        _focusedObject = gameObjects.OfType<Player>().First();
    }


    // render method to display the game state
    public void Render() {
        // clear the console
        Console.Clear();

        // display the current level and a tip
        var currentLevel = GetCurrentLevel() + 1;
        Console.WriteLine("HEXOBAN - Level " + currentLevel);
        helperMethods.DisplayTip(helperMethods.GetTip());

        // initialize the map
        map.Initialize();

        // place all game objects on the map
        PlaceGameObjects();

        // save the current map state to history
        map.SaveToHistory();

        // render the map
        for (int i = 0; i < map.MapHeight; i++) {
            for (int j = 0; j < map.MapWidth; j++) {
                DrawObject(map.Get(i, j));
            }
            Console.WriteLine();
        }
    }


    // method to create a game object using the factory
    public GameObject CreateGameObject(dynamic obj) {
        return gameObjectFactory.CreateGameObject(obj);
    }


    // method to add a game object to the game objects list
    public void AddGameObject(GameObject gameObject) {
        gameObjects.Add(gameObject);
    }


    // method to remove a game object from the game objects list
    public void RemoveGameObject(GameObject gameObject) {
        gameObjects.Remove(gameObject);
    }


    // method to place game objects on the map
    private void PlaceGameObjects() {
        // RENDER THE OBSTACLES (walls)
        gameObjects.ForEach(delegate(GameObject obj) {
            if (obj.Type == GameObjectType.Obstacle) {
                map.Set(ref obj);
            }
        });

        // RENDER THE TARGETS
        gameObjects.ForEach(delegate(GameObject obj) {
            if (obj.Type == GameObjectType.Target) {
                map.Set(ref obj);
            }
        });

        // RENDER THE BOXES
        gameObjects.ForEach(delegate(GameObject obj) {
            if (obj.Type == GameObjectType.Box) {
                map.Set(ref obj);
            }
        });

        // RENDER THE NPCs
        gameObjects.ForEach(delegate(GameObject obj) {
            if (obj.Type == GameObjectType.NPC) {
                map.Set(ref obj);
            }
        });

        // RENDER THE PLAYER
        gameObjects.ForEach(delegate(GameObject obj) {
            if (obj.Type == GameObjectType.Player) {
                map.Set(ref obj);
                return;
            }
        });
    }


    // method to draw a game object on the console
    private void DrawObject(GameObject gameObject) {
        Console.ResetColor();

        if (gameObject != null) {
            Console.ForegroundColor = gameObject.Color;
            Console.Write(gameObject.CharRepresentation);
        } else {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(' ');
        }
    }


    // update method to update the game state
    public void Update() {
        // check for collisions
        gameObjects.ForEach(delegate(GameObject obj) {
            if (obj.Type != GameObjectType.Floor) {
                if (map.Get(obj.PosY, obj.PosX) is GameObject gameObject && map.Get(obj.PosY, obj.PosX) != obj && map.Get(obj.PosY, obj.PosX).Type != GameObjectType.Floor) {
                    obj.onCollision(gameObject, map.GetMap());
                    map.Get(obj.PosY, obj.PosX).onCollision(obj, map.GetMap());
                }
            }
        });

        // check if history needs to be reset
        if (map.resetHistory) {
            map.revertHistory();

            // update positions of game objects based on map state
            gameObjects.ForEach(delegate(GameObject obj) {
                if (obj is Player) {
                    for (int i = 0; i < map.MapHeight; i++) {
                        for (int j = 0; j < map.MapWidth; j++) {
                            if (map.Get(i, j) is Player) {
                                obj.PosX = j;
                                obj.PosY = i;
                                break;
                            }
                        }
                    }
                } else if (obj is Box) {
                    for (int i = 0; i < map.MapHeight; i++) {
                        for (int j = 0; j < map.MapWidth; j++) {
                            if (map.Get(i, j) == obj) {
                                obj.PosX = j;
                                obj.PosY = i;
                                break;
                            }
                        }
                    }
                }
            });
        }
    }
}
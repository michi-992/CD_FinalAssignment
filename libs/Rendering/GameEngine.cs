﻿using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace libs;

using System.Security.Cryptography;
using Newtonsoft.Json;

public sealed class GameEngine
{
    private static GameEngine? _instance;
    private IGameObjectFactory gameObjectFactory;

    private List<NPC> npcs;

    public static GameEngine Instance {
        get{
            if(_instance == null)
            {
                _instance = new GameEngine();
            }
            return _instance;
        }
    }

    private GameEngine() {
        //INIT PROPS HERE IF NEEDED
        gameObjectFactory = new GameObjectFactory();
        npcs = new List<NPC>();
    }

    private GameObject? _focusedObject;

    private Map map = new Map();

    private List<GameObject> gameObjects = new List<GameObject>();

    public void SetNPCs(List<NPC> npcList)
    {
        npcs = npcList;
    }

    public List<NPC> GetNPCs()
    {
        return npcs;
    }

    public Map GetMap() {
        return map;
    }

    public GameObject GetFocusedObject(){
        return _focusedObject;
    }

    private int? initalGameLevel = 0; // initial level - gets changed if there is a saved game
    private int currentGameLevel = 0; // tracks game level and changes once nextLevel is called in Program

    // get and set for current game level int
    public int GetCurrentLevel() {
        return currentGameLevel;
    }
    public void SetCurrentLevel(int value) {
        currentGameLevel = value;
    }

    private bool resetGame = false; // tracks restart game key press

    // get and set for restartGame bool
    public bool GetRestartGame() {
        return resetGame;
    }

    public void SetRestartGame(bool restart) {
        resetGame = restart;
    }


    public void Setup(int currLevel = 0){
        //Added for proper display of game characters
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        dynamic gameData = FileHandler.ReadJson();
        dynamic gameDataSaved = FileHandler.ReadSavedJson();

        // checks whether there is a currently saved game and whether it is not bigger than the current level
        initalGameLevel = gameDataSaved.currentLevel; 
        if(initalGameLevel != null && initalGameLevel !> currLevel) SetCurrentLevel(initalGameLevel.Value);
        
        // checks whether SETUP gameObjects or saved game gameObjects should be used
        var gameObjectsJSON = gameData[currentGameLevel].gameObjects;
        if(gameDataSaved.gameObjects.Count > 0 && (initalGameLevel !> currLevel || initalGameLevel == currLevel)) {
            gameObjectsJSON = gameDataSaved.gameObjects;
        }
        
        map.MapWidth = gameData[currentGameLevel].map.width;
        map.MapHeight = gameData[currentGameLevel].map.height;


        gameObjects = new List<GameObject>();

        foreach (var gameObject in gameObjectsJSON)
        {
            AddGameObject(CreateGameObject(gameObject));
        }
        
        _focusedObject = gameObjects.OfType<Player>().First();

    }

    public void Render() {
        
        //Clean the map
        Console.Clear();

        map.Initialize();

        PlaceGameObjects();

        map.SaveToHistory();

        //Render the map
        for (int i = 0; i < map.MapHeight; i++)
        {
            for (int j = 0; j < map.MapWidth; j++)
            {
                DrawObject(map.Get(i, j));
            }
            Console.WriteLine();
        }

        foreach (var npc in npcs)
        {
            Console.SetCursorPosition(npc.PosX, npc.PosY);
            Console.ForegroundColor = npc.Color;
            Console.Write(npc.CharRepresentation);
        }
    }
    
    // Method to create GameObject using the factory from clients
    public GameObject CreateGameObject(dynamic obj)
    {
        return gameObjectFactory.CreateGameObject(obj);
    }

    public void AddGameObject(GameObject gameObject){
        gameObjects.Add(gameObject);
    }

    public void RemoveGameObject(GameObject gameObject){
        gameObjects.Remove(gameObject);
    }

    public void revertHistory() {
        map.resetHistory = true;
    }

    // for removing history once a level was completed 
    public void removeHistory() {
        map.clearHistory();
    }

    private void PlaceGameObjects(){

        // RENDER THE WALLS
        gameObjects.ForEach(delegate(GameObject obj)
        {
            if (obj.Type == GameObjectType.Obstacle)
            {
                map.Set(ref obj);
            }
        });

        // RENDER THE TARGETS
        gameObjects.ForEach(delegate(GameObject obj)
        {
            if (obj.Type == GameObjectType.Target)
            {
                map.Set(ref obj);
            }
        });

        // RENDER THE BOXES
        gameObjects.ForEach(delegate(GameObject obj)
        {
            if (obj.Type == GameObjectType.Box)
            {
                map.Set(ref obj);
            }
        });

        // RENDER THE PLAYER
        gameObjects.ForEach(delegate(GameObject obj)
        {
            if (obj.Type == GameObjectType.Player)
            {
                map.Set(ref obj);
                return;
            }
        });

        // RENDER THE NPCs
        gameObjects.ForEach(delegate(GameObject obj)
        {
            if (obj.Type == GameObjectType.NPC)
            {
                map.Set(ref obj);
                return;
            }
        });
    }

    private void DrawObject(GameObject gameObject){
        
        Console.ResetColor();

        if(gameObject != null)
        {
            Console.ForegroundColor = gameObject.Color;
            Console.Write(gameObject.CharRepresentation);
        }
        else{
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(' ');
        }
    }

    public void Update() {
        // CHECK COLLISIONS
        gameObjects.ForEach(delegate(GameObject obj)
        {
            if (obj.Type != GameObjectType.Floor) {
                if (map.Get(obj.PosY, obj.PosX) is GameObject gameObject && map.Get(obj.PosY, obj.PosX) != obj && map.Get(obj.PosY, obj.PosX).Type != GameObjectType.Floor) {
                    obj.onCollision(gameObject, map.GetMap());
                    map.Get(obj.PosY, obj.PosX).onCollision(obj, map.GetMap());
                }
            }
        });

        if (map.resetHistory) {
            map.revertHistory();

            gameObjects.ForEach(delegate(GameObject obj)
            {
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
                }
                else if (obj is Box) {
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

    public bool allTargetsFilled() {
        foreach (GameObject obj in gameObjects) {
            if (obj is Target) {
                if (map.Get(obj.PosY, obj.PosX).Type != GameObjectType.Box) {
                    return false;
                }
            }
        }
        return true;
    }

    public void saveGame() {
        List<GameObject> gameObjects = new List<GameObject>();
        // iterates over all gameobjects in last item of map.history and adds them to the list if not Floor
        for (int i = 0; i < map.MapWidth; i++) {
            for (int j = 0; j < map.MapHeight; j++) {
                if(map.history.Last()[j,i].Type != GameObjectType.Floor) gameObjects.Add(map.history.Last()[j,i]);
            }
        }

        // saves current level int and gameobjects in a file
        var gameState = new GameState { currentLevel = currentGameLevel, gameObjects =  gameObjects };
        string output = JsonConvert.SerializeObject(gameState);
        File.WriteAllText("../SavedFile.json", output);
    }

    public void loadGame()
    {
        try
        {
            // Read JSON content from SavedFile.json
            string jsonContent = File.ReadAllText("../SavedFile.json");

            // Deserialize JSON into GameState object
            GameState gameState = JsonConvert.DeserializeObject<GameState>(jsonContent);

            // Clear current game state
            gameObjects.Clear();

            // Set current level
            if (gameState.currentLevel.HasValue)
            {
                SetCurrentLevel(gameState.currentLevel.Value);
            }
            else
            {
                Console.WriteLine("Warning: Saved game state does not contain a valid current level.");
                // You might want to handle this case depending on your game logic
            }

            // Load game objects
            foreach (var gameObjectData in gameState.gameObjects)
            {
                // Create each game object and add to game state
                GameObject gameObject = CreateGameObject(gameObjectData);
                AddGameObject(gameObject);
            }

            Console.WriteLine("Successfully loaded the saved game state.");
            }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading saved game: {ex.Message}");
        }
    }


    // called with restart game key input (R)
    public void restartGame() {
        resetGame = true;
    }

}
namespace libs;
using Newtonsoft.Json;

// helperMethods class containing utility methods for the game engine
public class HelperMethods { 

    // method to get a gameplay tip based on the player's position relative to NPCs
    public string GetTip() {
        var engine = GameEngine.Instance; // get game engine instance
        Player player = engine.GetGameObjects().OfType<Player>().FirstOrDefault();
        
        // check if the player is next to an NPC and return a tip if true
        if (player != null && player.NextToNPC(engine.GetMap().GetMap())) {
            return "Press E to interact with NPC";
        }
        return string.Empty;
    }


    // method to display a tip in the console
    public void DisplayTip(string tip) {
        // clear the tip line
        Console.SetCursorPosition(0, 1);
        Console.WriteLine(new string(' ', Console.WindowWidth));
        
        // display the tip
        Console.SetCursorPosition(0, 1);
        Console.WriteLine(tip);
    }


    // method to create a dialog object from dynamic data
    public Dialog? createDialog(dynamic currDialog) {
        if (currDialog.Count != 0) {
            // create an array of dialog nodes
            DialogNode[] options = new DialogNode[currDialog.Count];

            // initialize each dialog node
            for (int i = 0; i < currDialog.Count; i++) {
                options[i] = new DialogNode((string)currDialog[i].text);
            }

            // add responses to each dialog node
            for (int i = 0; i < currDialog.Count; i++) {
                if (currDialog[i].responses != null) {
                    foreach (var response in currDialog[i].responses) {
                        options[i].AddResponse((string)response.text, options[response.nextNode - 1]);
                    }
                }
            }

            // return the first dialog node
            return new Dialog(options[0]);
        } else {
            return null;
        }
    }


    // method to save the current game state to a file
    public void saveGame() {
        // get the game engine instance
        var engine = GameEngine.Instance;

        // iterate over the map history and add game objects to the list if they are not Floor
        List<GameObject> gameObjects = new List<GameObject>();
        for (int i = 0; i < engine.GetMap().MapWidth; i++) {
            for (int j = 0; j < engine.GetMap().MapHeight; j++) {
                if (engine.GetMap().history.Last()[j, i].Type == GameObjectType.Player || engine.GetMap().history.Last()[j, i].Type == GameObjectType.Box) {
                    gameObjects.Add(engine.GetMap().history.Last()[j, i]);
                }
            }
        }
        
        // save the current level and game objects to a file
        var gameState = new GameState { currentLevel = engine.GetCurrentLevel(), gameObjects = gameObjects };
        string output = JsonConvert.SerializeObject(gameState);
        File.WriteAllText("../SavedFile.json", output);
    }


    // method to check if all targets are filled with the correct boxes
    public bool allTargetsFilled() {
        var engine = GameEngine.Instance;

        // iterate over all game objects and check if each target has a box on it
        foreach (GameObject obj in engine.GetGameObjects()) {
            if (obj is Target) {
                if (engine.GetMap().Get(obj.PosY, obj.PosX).Type != GameObjectType.Box || engine.GetMap().Get(obj.PosY, obj.PosX).CharRepresentation != obj.CharRepresentation) {
                    return false;
                }
            }
        }
        return true;
    }


    // method to update game objects' (player and boxes) positions from saved data
    public dynamic UseSavedObjects(dynamic gameObjectsJSON, dynamic gameDataSaved) {
        int currentIndex = 0;

       
        for (int i = 0; i < gameObjectsJSON.Count; i++) {
             // update positions of player
            if (gameObjectsJSON[i].Type == 0) {
                for (int j = 0; j < gameDataSaved.gameObjects.Count; j++) {
                    if (gameObjectsJSON[i].Type == gameDataSaved.gameObjects[j].Type) {
                        gameObjectsJSON[i].PosX = gameDataSaved.gameObjects[j].PosX;
                        gameObjectsJSON[i].PosY = gameDataSaved.gameObjects[j].PosY;
                        break;
                    }
                }
            }

            // update positions of boxes and character representation
            if (gameObjectsJSON[i].Type == 2) {
                for (int j = currentIndex; j < gameDataSaved.gameObjects.Count; j++) {
                    if (gameObjectsJSON[i].Type == gameDataSaved.gameObjects[j].Type) {
                        gameObjectsJSON[i].PosX = gameDataSaved.gameObjects[j].PosX;
                        gameObjectsJSON[i].PosY = gameDataSaved.gameObjects[j].PosY;
                        gameObjectsJSON[i].CharRepresentation = gameDataSaved.gameObjects[j].CharRepresentation;
                        currentIndex++;
                        break;
                    } else {
                        currentIndex++;
                    }
                }
            }
        }

        return gameObjectsJSON;
    }
}

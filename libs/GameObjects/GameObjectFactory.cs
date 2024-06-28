namespace libs;

// GameObjectFactory class
public class GameObjectFactory : IGameObjectFactory
{
    // creates GameObject based on dynamic input
    public GameObject CreateGameObject(dynamic obj)
    {
        GameObject newObj = new GameObject(); // Create a new generic GameObject
        int type = obj.Type; // Get the type of the object from the dynamic input

        switch (type)
        {
            // converts object types
            case (int)GameObjectType.Player:
                newObj = Player.Instance; // uses the singleton instance
                newObj.PosX = obj.PosX;   // sets x position from dynamic object
                newObj.PosY = obj.PosY;   // sets y position from dynamic object
                newObj.Color = obj.Color; // sets color from dynamic object
                break;
            case (int)GameObjectType.Obstacle:
                newObj = obj.ToObject<Obstacle>();
                break;
            case (int)GameObjectType.Box:
                newObj = obj.ToObject<Box>();
                break;
            case (int)GameObjectType.Target:
                newObj = obj.ToObject<Target>();
                break;
            case (int)GameObjectType.NPC:
                newObj = obj.ToObject<NPC>();
                break;
        }

        return newObj; // Return the created or converted GameObject
    }
}

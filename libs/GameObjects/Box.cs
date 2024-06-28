namespace libs;

// Inherits from GameObject
public class Box : GameObject {

    public Box () : base(){
        Type = GameObjectType.Player; // sets GameObjectType to Player
        CharRepresentation = '○';
        Color = ConsoleColor.Magenta;
    }
}
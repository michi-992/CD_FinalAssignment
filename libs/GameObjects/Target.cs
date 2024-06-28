namespace libs;

// inherits from GameObject
public class Target : GameObject {

    public Target () : base(){
        Type = GameObjectType.Target; // sets type to target
        CharRepresentation = 'X';
        Color = ConsoleColor.Green;
    }
}
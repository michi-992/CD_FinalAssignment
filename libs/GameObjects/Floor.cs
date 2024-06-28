namespace libs;

// inherits from GameObject
public class Floor : GameObject {

    // Constructor, invokes base constructor of GameObject
    public Floor () : base(){
        this.Type = GameObjectType.Floor; // sets GameObjectType
        this.CharRepresentation = '.';
        this.Color = ConsoleColor.Gray;
    }
}
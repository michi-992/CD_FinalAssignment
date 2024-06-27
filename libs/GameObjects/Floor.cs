namespace libs;

public class Floor : GameObject {

    public Floor () : base(){
        this.Type = GameObjectType.Floor;
        this.CharRepresentation = '.';
        this.Color = ConsoleColor.Gray;
    }
}
namespace libs;

// inherits from GameObject
public class NPC : GameObject {

    public NPC () : base(){
        this.Type = GameObjectType.NPC; // sets type to NPC
        this.CharRepresentation = '☻';
        this.Color = ConsoleColor.DarkYellow;
    }
}
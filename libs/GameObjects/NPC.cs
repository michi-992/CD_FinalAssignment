namespace libs;

public class NPC : GameObject {

    public NPC () : base(){
        this.Type = GameObjectType.NPC;
        this.CharRepresentation = '☻';
        this.Color = ConsoleColor.DarkYellow;
    }
}
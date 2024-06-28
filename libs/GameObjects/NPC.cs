namespace libs;

// inherits from GameObject
public sealed class NPC : GameObject {
    private static NPC _instance = null; // singleton instance

    // accessing singleton instance
    public static NPC Instance {
        get{
            if(_instance == null)
            {
                _instance = new NPC(); // returns new instance if there isn't one
            }
            return _instance; // returns existing instance
        }
    }

    public NPC () : base(){
        this.Type = GameObjectType.NPC; // sets type to NPC
        this.CharRepresentation = '☻';
    }
}
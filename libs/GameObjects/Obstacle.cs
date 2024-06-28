namespace libs;

// inherits from GameObject
public class Obstacle : GameObject {
    public Obstacle () : base() {
        this.Type = GameObjectType.Obstacle; // sets type to Obstacle
        this.CharRepresentation = '█';
        this.Color = ConsoleColor.DarkGray;
    }
}
namespace libs;

// sealed class, inherits from GameObject
public sealed class Player : GameObject {
    private static Player _instance = null; // singleton instance

    // accessing singleton instance
    public static Player Instance {
        get{
            if(_instance == null)
            {
                _instance = new Player(); // returns new instance if there isn't one
            }
            return _instance; // returns existing instance
        }
    }

    private Player () : base(){
        Type = GameObjectType.Player; // sets type to Player
        CharRepresentation = '☻';
    }

    // collision handling
    public override void onCollision(GameObject gameObject, GameObject?[,] map) {
        if (gameObject.Type == GameObjectType.Obstacle) {
            // Reset position to previous position if colliding with Obstacle or NPC
            this.PosX = this.GetPrevPosX();
            this.PosY = this.GetPrevPosY();
        }
        else if (gameObject.Type == GameObjectType.NPC) {
            this.PosX = this.GetPrevPosX();
            this.PosY = this.GetPrevPosY();
        }
        else if (gameObject.Type == GameObjectType.Box) {
            bool moved = false;
            int posX = this.PosX + (this.PosX - this.GetPrevPosX());
            int posY = this.PosY + (this.PosY - this.GetPrevPosY());

            // moves the Box in the direction Player was moving
            while (!(map[posY, posX] is Obstacle || map[posY, posX] is Box)) {
                gameObject.Move(this.PosX - this.GetPrevPosX(), this.PosY - this.GetPrevPosY());
                moved = true;
                posX += (this.PosX - this.GetPrevPosX());
                posY += (this.PosY - this.GetPrevPosY());
                break;
            }

            // AVOID PLAYER MOVEMENT IF BOXES CANNOT BE MOVED
            if (!moved) {
                this.PosX = this.GetPrevPosX();
                this.PosY = this.GetPrevPosY();
            }
        }
    }

    // Checks if player is next to an NPC
    public bool NextToNPC(GameObject?[,] map) {
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        // checks adjacent positions for NPC
        for (int i = 0; i < 4; i++) {
            int newX = this.PosX + dx[i];
            int newY = this.PosY + dy[i];
            if (map[newY, newX] != null && map[newY, newX].Type == GameObjectType.NPC) {
                return true;
            }
        }
        return false; // if no NPC is adjacent
    }
}
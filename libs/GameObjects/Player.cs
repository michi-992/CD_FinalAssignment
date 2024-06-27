namespace libs;

public sealed class Player : GameObject {
    private static Player _instance = null;

    public static Player Instance {
        get{
            if(_instance == null)
            {
                _instance = new Player();
            }
            return _instance;
        }
    }

    private Player () : base(){
        Type = GameObjectType.Player;
        CharRepresentation = '☻';
        Color = ConsoleColor.DarkYellow;
    }

    public override void onCollision(GameObject gameObject, GameObject?[,] map) {
        if (gameObject.Type == GameObjectType.Obstacle) {
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
    public bool NextToNPC(GameObject?[,] map) {
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++) {
            int newX = this.PosX + dx[i];
            int newY = this.PosY + dy[i];
            if (map[newY, newX] != null && map[newY, newX].Type == GameObjectType.NPC) {
                return true;
            }
        }
        return false;
    }
}
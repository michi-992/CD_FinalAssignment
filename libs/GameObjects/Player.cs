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
}
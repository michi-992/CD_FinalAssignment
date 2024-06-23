using System;
using System.Threading;
using System.Collections.Generic;

namespace libs;

public class NPC : GameObject
{
    public Dialog Dialog { get; private set; }

    public NPC(int x, int y, DialogNode startingNode) : base()
    {
        Type = GameObjectType.NPC;
        CharRepresentation = 'N';
        Color = ConsoleColor.Cyan;
        PosX = x;
        PosY = y;
        Dialog = new Dialog(startingNode);
    }
}
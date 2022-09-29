using Board.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardInput : ScriptableObject
{
    public virtual string Id { get; protected set; }

    public abstract void ProcessInput(BoardController controller, Vector2Int position);
}

using Board.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public virtual string Id { get; protected set; }

    public abstract void PerformSkill(BoardController controller, Vector2Int position, int color);
}

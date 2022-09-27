using Board.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Input")]
public abstract class BoardInput : ScriptableObject
{
    public virtual string Id { get; protected set; }

    public abstract void ProcessInput(BoardController controller);
}

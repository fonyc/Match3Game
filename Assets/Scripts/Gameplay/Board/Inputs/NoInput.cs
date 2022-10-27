using Board.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Input/NoInput")]
public class NoInput : BoardInput
{
    public override string Id { get { return "NoInput"; } }

    public override void ProcessInput(BoardController controller, Vector2Int position)
    {
        return;
    }
}

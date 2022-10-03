using Board.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Input/NormalInput")]
public class NormalInput : BoardInput
{
    public override string Id { get { return "NormalInput"; } }

    public override void ProcessInput(BoardController controller, Vector2Int position)
    {
        controller.TryProcessMatch(position);
    }
}

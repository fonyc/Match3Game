using Board.Controller;
using Board.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Decolorate")]
public class DecolorateSkill : Skill
{
    public override string Id { get { return "Decolorate"; } }

    public override void PerformSkill(BoardController controller, Vector2Int position, int color)
    {
        controller.ChangeEmblemColorAtPosition(position, color);
        controller.UpdateMoves();
    }
}

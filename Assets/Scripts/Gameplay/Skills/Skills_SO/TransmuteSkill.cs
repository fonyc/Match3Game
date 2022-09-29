using Board.Controller;
using Board.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Transmute")]
public class TransmuteSkill : Skill
{
    public override string Id { get { return "Transmute"; } }

    public override void PerformSkill(BoardController controller, Vector2Int position, int color)
    {
        List<EmblemModel> emblems = new();
        EmblemModel emblem = controller.GetEmblem(position);
        emblems.Add(emblem);
        controller.DestroyAndCollapse(emblems);
        controller.CreateEmblemAtPosition(position, color);
    }
}

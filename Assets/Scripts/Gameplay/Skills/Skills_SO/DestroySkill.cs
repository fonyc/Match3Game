using Board.Controller;
using Board.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/DestroySkill")]
public class DestroySkill : Skill
{
    public override string Id => "Destroy";

    public override void PerformSkill(BoardController controller, Vector2Int position, int color)
    {
        List<EmblemModel> emblems = new();
        EmblemModel emblem = controller.GetEmblem(position);
        emblems.Add(emblem);
        controller.DestroyAndCollapse(emblems);
    }
}

using Board.Controller;
using Board.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/RefillSkill")]
public class RefillSkill : Skill
{
    public override string Id { get { return "Refill"; } }

    public override void PerformSkill(BoardController controller, Vector2Int position, int color)
    {
        controller.RefillBoard();
    }
}

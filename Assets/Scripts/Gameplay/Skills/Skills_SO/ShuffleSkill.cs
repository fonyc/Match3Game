using Board.Controller;
using Board.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/ShuffleSkill")]
public class ShuffleSkill : Skill
{
    public override string Id { get { return "Shuffle"; } }

    public override void PerformSkill(BoardController controller, Vector2Int position, int color)
    {
        controller.ShuffleBoard();
    }
}

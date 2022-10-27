using Board.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Input/SkillInput")]
public class SkillInput : BoardInput
{
    public override string Id { get { return "SkillInput"; } }

    public override void ProcessInput(BoardController controller, Vector2Int position)
    {
        controller.TryUseSkill(position);
    }
}

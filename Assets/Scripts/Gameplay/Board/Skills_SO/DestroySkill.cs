using Board.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/DestroySkill")]
public class DestroySkill : Skill
{
    public override string Id { get { return "Destroy"; } }

    public override void PerformSkill(BoardController controller)
    {
        controller.DestroySkill();
    }
}

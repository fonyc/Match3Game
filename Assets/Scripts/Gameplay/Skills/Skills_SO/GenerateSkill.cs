using Board.Controller;
using Board.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/GenerateSkill")]
public class GenerateSkill : Skill
{
    public override string Id { get { return "Create"; } }

    public override void PerformSkill(BoardController controller, Vector2Int position, int color)
    {
        controller.CreateEmblemAtPosition(position, color);
    }
}

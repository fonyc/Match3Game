using UnityEngine;

public class SkillManager : MonoBehaviour
{
    Board board;

    private void Awake()
    {
        board = GetComponent<Board>();    
    }

    private void ExecuteSkill(int skill)
    {
        switch (skill)
        {
            case 0:
                HorizontalSkill hSkill = new();
                //hSkill.RemoveEmblems(board, );
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }
}

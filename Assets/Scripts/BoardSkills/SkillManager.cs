using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    Board _board;

    [SerializeField] Emblem _emblemSelected;

    [SerializeField] private ITileRemover _skillSelected = null;

    private void Awake()
    {
        _board = GetComponent<Board>();
    }
    public void SetEmblemSelected(Emblem emblem)
    {
        _emblemSelected = emblem;
        UseSkill(_skillSelected);

        _emblemSelected = null;
        _skillSelected = null;
    }

    public void SetSkill(int skill)
    {
        switch ((BoardSkills)skill)
        {
            case BoardSkills.Vertical:
                _skillSelected = new VerticalSkill();
                break;
            case BoardSkills.Horizontal:
                _skillSelected = new HorizontalSkill();
                break;
            case BoardSkills.Cross:
                _skillSelected = new CrossSkill();
                break;
        }
    }

    public void UseSkill(ITileRemover skill)
    {
        if (_emblemSelected == null || _skillSelected == null) return;
        _board.MatchFinder.CurrentMatches = skill.RemoveEmblems(_board, _emblemSelected);

        //Start a destruction chain
        _board.currentState = BoardStates.Wait;
        _board.DestroyMatches();
    }
}

using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    OldBoard _board;
    CombatManager _combatManager;

    [SerializeField] Emblem _emblemSelected;

    [SerializeField] private ITileRemover _skillSelected = null;

    [SerializeField] private DoubleIntArgument_Event _OnPlayerCrossManaChanged;
    [SerializeField] private DoubleIntArgument_Event _OnPlayerVerticalManaChanged;
    [SerializeField] private DoubleIntArgument_Event _OnPlayerHorizontalManaChanged;

    [SerializeField] private BoolArgument_Event _OnCrossManaSkillReady;
    [SerializeField] private BoolArgument_Event _OnVerticalManaSkillReady;
    [SerializeField] private BoolArgument_Event _OnHorizontalManaSkillReady;

    int maxMana;
    int currentMana;

    private void Awake()
    {
        _board = GetComponent<OldBoard>();
        _combatManager = GetComponent<CombatManager>();
    }

    public void SelectEmblemToPerformSkill(Emblem emblem)
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
        if (!CheckEnoughMana(skill, _combatManager.HERO)) return;
        _board.MatchFinder.CurrentMatches = skill.RemoveEmblems(_board, _emblemSelected, _combatManager.HERO);

        //Update Mana
        Hero hero = _combatManager.HERO;
        _OnPlayerCrossManaChanged.TriggerEvents(hero.currentCrossMana, hero.crossMana);
        _OnPlayerHorizontalManaChanged.TriggerEvents(hero.currentHorizontalMana, hero.horizontalMana);
        _OnPlayerVerticalManaChanged.TriggerEvents(hero.currentVerticalMana, hero.verticalMana);

        _OnCrossManaSkillReady.TriggerEvents(hero.currentCrossMana < hero.crossMana);
        _OnVerticalManaSkillReady.TriggerEvents(hero.currentVerticalMana < hero.verticalMana);
        _OnHorizontalManaSkillReady.TriggerEvents(hero.currentHorizontalMana < hero.horizontalMana);

        //Perform attack to enemy
        _board.MatchFinder.SendSimplifiedAttackReport();

        //Start a destruction chain
        _board.currentState = BoardStates.Wait;
        _board.DestroyMatches();
    }

    private bool CheckEnoughMana(object skill, Hero hero)
    {
        switch (skill)
        {
            case VerticalSkill:
                maxMana = hero.verticalMana;
                currentMana = hero.currentVerticalMana;
                break;
            case HorizontalSkill:
                maxMana = hero.horizontalMana;
                currentMana = hero.currentHorizontalMana;
                break;
            case CrossSkill:
                maxMana = hero.crossMana;
                currentMana = hero.currentCrossMana;
                break;
            default:
                return false;

        }
        Debug.Log(currentMana == maxMana);
        return currentMana == maxMana;
    }
}

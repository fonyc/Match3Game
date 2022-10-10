using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillController
{
    public event Action<string> OnSkillActivated = delegate (string input) { };

    private UserData _userData;

    private SkillPlayerModel _skillPlayerModel;
    private List<Skill> _skillBehaviours;
    public Skill SkillSelected;
    private GameConfigService _gameConfigService;

    public event Action<int, int> OnManaChanged = delegate (int mana, int maxMana) { };

    //public event Action<int> OnManaChanged = delegate (int mana) { };

    public SkillController(UserData userData, List<Skill> skillList, GameConfigService gameConfigService)
    {
        _gameConfigService = gameConfigService;
        _skillBehaviours = skillList;
        _userData = userData;
        _skillPlayerModel = new SkillPlayerModel();
    }

    public void AddMana(int hits, int manaPerHit)
    {
        int mana = hits * manaPerHit;
        int maxMana = _skillPlayerModel.Skill.Mana;
        int currentMana = _skillPlayerModel.playerCurrentMana;
        if (currentMana == maxMana) return;

        _skillPlayerModel.playerCurrentMana = currentMana + mana >= maxMana ? maxMana : currentMana + mana;
        OnManaChanged.Invoke(_skillPlayerModel.playerCurrentMana, _skillPlayerModel.Skill.Mana);
    }

    public void PerformSkill()
    {
        if (_skillPlayerModel.playerCurrentMana < _skillPlayerModel.Skill.Mana) return;
        _skillPlayerModel.playerCurrentMana = 0;
        OnManaChanged(_skillPlayerModel.playerCurrentMana, _skillPlayerModel.Skill.Mana);
        OnSkillActivated.Invoke("SkillInput");
    }

    private Skill GetSkill(string skillId)
    {
        return _skillBehaviours.Find(skill => skill.Id == skillId);
    }

    public void Initialize()
    {
        LoadSkill();
        SkillSelected = GetSkill(_skillPlayerModel.Skill.Id);
    }

    public SkillItemModel GetSkillItemModel()
    {
        return _skillPlayerModel.Skill;
    }

    public int GetCurrentPlayerMana()
    {
        return _skillPlayerModel.playerCurrentMana;
    }

    private void LoadSkill()
    {
        _skillPlayerModel = new SkillPlayerModel();

        List<SkillItemModel> allSkills = _gameConfigService.SkillModel;

        List<HeroItemModel> allHeroes = _gameConfigService.HeroModel;

        HeroItemModel heroSelected = GetHeroModelFromHeroName(_userData.GetSelectedHero(), allHeroes);

        _skillPlayerModel.Skill = GetSkill(heroSelected.Skill, allSkills);
        _skillPlayerModel.playerCurrentMana = 0;
    }

    private HeroItemModel GetHeroModelFromHeroName(string heroName, List<HeroItemModel> allHeroes)
    {
        foreach (HeroItemModel hero in allHeroes)
        {
            if (hero.Id == heroName) return hero;
        }
        return null;
    }

    private SkillItemModel GetSkill(string skillName, List<SkillItemModel> allSkills)
    {
        foreach (SkillItemModel skill in allSkills)
        {
            if (skill.Id == skillName) return skill;
        }
        return null;
    }
}

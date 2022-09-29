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

    public SkillController(UserData userData, List<Skill> skillList)
    {
        _skillBehaviours = skillList;
        _userData = userData;
        _skillPlayerModel = new SkillPlayerModel();
    }

    public void PerformSkill()
    {
        //if (_skillPlayerModel.playerCurrentMana < _skillPlayerModel.Skill.Mana) return;
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
        SkillModel allSkills = JsonUtility.FromJson<SkillModel>(Resources.Load<TextAsset>("SkillModel").text);
        HeroModel allheroes = JsonUtility.FromJson<HeroModel>(Resources.Load<TextAsset>("HeroModel").text);
        HeroItemModel heroSelected = GetHeroModelFromHeroName(_userData.GetSelectedHero(), allheroes);
        _skillPlayerModel.Skill = GetSkill(heroSelected.Skill, allSkills);
        _skillPlayerModel.playerCurrentMana = 0;
    }

    private HeroItemModel GetHeroModelFromHeroName(string heroName, HeroModel allHeroes)
    {
        foreach (HeroItemModel hero in allHeroes.Heroes)
        {
            if (hero.Id == heroName) return hero;
        }
        return null;
    }

    private SkillItemModel GetSkill(string skillName, SkillModel allSkills)
    {
        foreach (SkillItemModel skill in allSkills.Skills)
        {
            if (skill.Id == skillName) return skill;
        }
        return null;
    }
}

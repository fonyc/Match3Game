using Board.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController
{
    public event Action OnDestroyActivated = delegate () { };

    private UserData _userData;

    private SkillPlayerModel skillPlayerModel;

    public SkillController(UserData userData)
    {
        _userData = userData;
        skillPlayerModel = new SkillPlayerModel();
    }

    public void PerformSkill()
    {
        if (skillPlayerModel.playerCurrentMana < skillPlayerModel.Skill.Mana) return;
        switch (skillPlayerModel.Skill.Id)
        {
            case "Destroy":
                OnDestroyActivated?.Invoke();
                break;
            case "Decolorate":
                break;
            case "Swap":
                break;
            case "Create":
                break;
            case "Shuffle":
                break;
        }
        OnDestroyActivated?.Invoke();
    }

    public void Initialize()
    {
        LoadSkill();
    }

    public SkillItemModel GetSkill()
    {
        return skillPlayerModel.Skill;
    }

    public int GetCurrentPlayerMana()
    {
        return skillPlayerModel.playerCurrentMana;
    }

    private void LoadSkill()
    {
        SkillModel allSkills = JsonUtility.FromJson<SkillModel>(Resources.Load<TextAsset>("SkillModel").text);
        HeroModel allheroes = JsonUtility.FromJson<HeroModel>(Resources.Load<TextAsset>("HeroModel").text);
        HeroItemModel heroSelected = GetHeroModelFromHeroName(_userData.GetSelectedHero(), allheroes);
        skillPlayerModel.Skill = GetSkill(heroSelected.Skill, allSkills);
        skillPlayerModel.playerCurrentMana = 0;
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

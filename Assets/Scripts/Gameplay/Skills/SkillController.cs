using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController 
{
    private UserData _userData;

    private SkillPlayerModel skillPlayerModel;

    public SkillController(UserData userData)
    {
        _userData = userData;
        skillPlayerModel = new SkillPlayerModel();
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
        foreach(SkillItemModel skill in allSkills.Skills)
        {
            if (skill.Id == skillName) return skill;
        }
        return null;
    }
}

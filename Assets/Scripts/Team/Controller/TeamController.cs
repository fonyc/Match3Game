using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController 
{
    public HeroModel heroModel;

    public BattleItemsModel battleItemModel;

    //private BattleItemModel[] itemsSelected;

    private UserData _userData;

    public TeamController(UserData userData)
    {
        _userData = userData;
    }

    public void SelectHero(string heroName)
    {
        _userData.SelectHero(heroName);
        _userData.Save();
    }

    public void SelectItem(string itemName)
    {
        _userData.SelectItem(itemName);
        _userData.Save();
    }

    public void Initialize()
    {
        Load();
    }

    private void Load()
    {
        heroModel = JsonUtility.FromJson<HeroModel>(Resources.Load<TextAsset>("HeroModel").text);
        battleItemModel = JsonUtility.FromJson<BattleItemsModel>(Resources.Load<TextAsset>("BattleItemModel").text);
    }
}

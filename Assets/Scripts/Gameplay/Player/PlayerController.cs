using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController
{
    private UserData _userData;
    private PlayerModel _playerModel;

    public PlayerController(UserData userData)
    {
        _playerModel = new PlayerModel();
        _userData = userData;
    }

    private HeroItemModel GetHeroData(HeroModel allHeroesModel)
    {
        foreach (HeroItemModel hero in allHeroesModel.Heroes)
        {
            if (hero.Id == _userData.GetSelectedHero()) return hero;
        }
        return null;
    }

    private List<BattleItemModel> GetBattleItemData(BattleItemsModel allItemsModel)
    {
        List<BattleItemModel> result = new();

        foreach (string selectedItem in _userData.GetSelectedItems())
        {
            foreach (BattleItemModel item in allItemsModel.BattleItems)
            {
                if (result.Count >= 2) return result;
                if (item.Id == selectedItem) result.Add(item);
            }
        }
        result.Distinct().ToList();
        return result;
    }

    public void Initialize()
    {
        LoadSelectedHero();
        LoadSelectedItems();
    }

    private void LoadSelectedHero()
    {
        HeroModel allHeroesModel = JsonUtility.FromJson<HeroModel>(Resources.Load<TextAsset>("HeroModel").text);
       _playerModel.hero = GetHeroData(allHeroesModel);
    }

    private void LoadSelectedItems()
    {
        BattleItemsModel allItemsModel = JsonUtility.FromJson<BattleItemsModel>(Resources.Load<TextAsset>("BattleItemModel").text);
        _playerModel.items = GetBattleItemData(allItemsModel);
    }
}
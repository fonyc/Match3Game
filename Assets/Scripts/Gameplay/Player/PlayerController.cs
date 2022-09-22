using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerController
{
    private UserData _userData;
    private PlayerModel _playerModel;

    public PlayerController(UserData userData)
    {
        _userData = userData;
        _playerModel = new PlayerModel();
        Initialize();
    }

    #region HEROES

    public HeroItemModel GetHero()
    {
        return _playerModel.hero;
    }

    public HeroStats GetCurrentStats()
    {
        return _playerModel.currentHeroStats;
    }

    private HeroItemModel GetHeroData(HeroModel allHeroesModel)
    {
        foreach (HeroItemModel hero in allHeroesModel.Heroes)
        {
            if (hero.Id == _userData.GetSelectedHero()) return hero;
        }
        return null;
    }
    #endregion

    #region ITEMS
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

    public BattleItemModel GetItem(int number)
    {
        return _playerModel.itemStats[number];
    }
    #endregion


    public void Initialize()
    {
        LoadSelectedHero();
        LoadSelectedItems();
    }

    private void LoadSelectedHero()
    {
        HeroModel allHeroesModel = JsonUtility.FromJson<HeroModel>(Resources.Load<TextAsset>("HeroModel").text);
        _playerModel.hero = GetHeroData(allHeroesModel);
        _playerModel.currentHeroStats = _playerModel.hero.Stats;
    }

    private void LoadSelectedItems()
    {
        BattleItemsModel allItemsModel = JsonUtility.FromJson<BattleItemsModel>(Resources.Load<TextAsset>("BattleItemModel").text);
        _playerModel.itemStats = GetBattleItemData(allItemsModel);
        //_playerModel.slot1Potions = Mathf.Max(GetHero().MaxItems, _playerModel.items[0].Qa);
    }
}
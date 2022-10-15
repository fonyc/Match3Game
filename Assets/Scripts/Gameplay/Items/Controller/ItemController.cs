using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemController 
{
    public event Action<int> _onManaItemConsumed = delegate (int amount) { };
    public event Action<int> _onHPItemConsumed = delegate (int amount) { };
    public event Action<int> _onATKItemConsumed = delegate (int amount) { };
    public event Action<int> _onDEFItemConsumed = delegate (int amount) { };

    private UserData _userData;
    public ItemModel Model;
    private GameConfigService _gameConfigService;

    public ItemController(UserData userData, GameConfigService gameConfigService)
    {
        _gameConfigService = gameConfigService;
        _userData = userData;
        Model = new ItemModel();
    }

    public void RemovePotionFromPlayer(string itemName)
    {
        _userData.RemoveBattleItem(itemName);
        _userData.Save();
    }

    public void OnPlayerStatChanged(string stat, int amount)
    {
        switch (stat)
        {
            case "ATK":
                _onATKItemConsumed?.Invoke(amount);
                break;
            case "DEF":
                _onDEFItemConsumed?.Invoke(amount);
                break;
            case "HP":
                _onHPItemConsumed?.Invoke(amount);
                break;
            default:
                _onManaItemConsumed.Invoke(amount);
                break;
        }
    }

    public void Initialize()
    {
        LoadModel();
    }

    private void LoadModel()
    {
        Model = new ItemModel();
        List<BattleItemModel> allItemsModel = _gameConfigService.BattleItemsModel;
        Model.itemStats = GetBattleItemData(allItemsModel);
        Model.selectedItems = GetSelectedBattleItems(_userData);

        List<HeroItemModel> allHeroesModel = _gameConfigService.HeroModel;
        Model.MaxItemQty = GetMaxItems(_userData.GetSelectedHero(), allHeroesModel);
    }

    private List<OwnedBattleItem> GetSelectedBattleItems(UserData userData)
    {
        List<OwnedBattleItem> result = new();
        foreach (OwnedBattleItem ownedItem in userData.GetOwnedBattleItems())
        {
            foreach (string selectedItem in userData.GetSelectedItems())
            {
                if (ownedItem.Id == selectedItem) result.Add(ownedItem);
            }
        }
        return result;
    }

    private List<BattleItemModel> GetBattleItemData(List<BattleItemModel> allItemsModel)
    {
        List<BattleItemModel> result = new();

        foreach (string selectedItem in _userData.GetSelectedItems())
        {
            foreach (BattleItemModel item in allItemsModel)
            {
                if (result.Count >= 2) return result;
                if (item.Id == selectedItem) result.Add(item);
            }
        }
        result.Distinct().ToList();
        return result;
    }

    private int GetMaxItems(string heroName, List<HeroItemModel> heroList)
    {
        foreach(HeroItemModel hero in heroList)
        {
            if (hero.Id == heroName) return hero.MaxItems;
        }
        return 0;
    }
}

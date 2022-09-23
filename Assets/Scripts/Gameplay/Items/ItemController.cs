using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemController 
{
    private UserData _userData;
    public ItemModel Model;

    public ItemController(UserData userData)
    {
        _userData = userData;
        Model = new ItemModel();
    }

    public void Initialize()
    {
        LoadModel();
    }

    private void LoadModel()
    {
        BattleItemsModel allItemsModel = JsonUtility.FromJson<BattleItemsModel>(Resources.Load<TextAsset>("BattleItemModel").text);
        Model.itemStats = GetBattleItemData(allItemsModel);
        Model.selectedItems = GetSelectedBattleItems(_userData);

        HeroModel allHeroesModel = JsonUtility.FromJson<HeroModel>(Resources.Load<TextAsset>("HeroModel").text);
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

    private int GetMaxItems(string heroName, HeroModel heroList)
    {
        foreach(HeroItemModel hero in heroList.Heroes)
        {
            if (hero.Id == heroName) return hero.MaxItems;
        }
        return 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamController 
{
    public HeroModel heroModel;

    public BattleItemsModel battleItemModel;

    private List<string> itemsSelected = new();

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
        List<string> selectedItems = _userData.GetSelectedItems();

        if (selectedItems.Contains(itemName)) _userData.DeselectItem(itemName);
        else if (selectedItems.Count < 2) _userData.SelectItem(itemName);
        else 
        {
            _userData.DeselectItem(selectedItems.Last());
            _userData.SelectItem(itemName);
        }

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

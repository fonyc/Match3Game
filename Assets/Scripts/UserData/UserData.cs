using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class UserData
{
    [SerializeField] private List<ResourceItem> Items = new();

    [SerializeField] private List<OwnedHero> Heroes = new();

    [SerializeField] private List<OwnedBattleItem> BattleItems = new();

    [SerializeField] private string SelectedHero = null;

    [SerializeField] private List<string> SelectedItems = new();

    [SerializeField] private int levelsPassed;

    [SerializeField] private int currentSelectedLevel;

    private string path = Application.persistentDataPath + "/userData.json";

    #region EVENTS
    public event Action<string> OnResourceModified = resource => { };
    public event Action<string> OnHeroModified = hero => { };
    public event Action OnHeroAdded;
    public event Action OnBattleItemModified;
    public event Action OnBattleItemAdded;
    public event Action OnBattleItemSelected;
    public event Action OnBattleItemDeSelected;
    #endregion

    #region RESOURCES

    public void AddResource(ResourceItem item)
    {
        foreach (ResourceItem resourceItem in Items)
        {
            if (resourceItem.Name == item.Name)
            {
                resourceItem.Amount += item.Amount;
                OnResourceModified(item.Name);
                return;
            }
        }

        Items.Add(new ResourceItem { Name = item.Name, Type = item.Type, Amount = item.Amount });
        OnResourceModified(item.Name);
    }

    public void RemoveResource(ResourceItem item)
    {
        foreach (ResourceItem resourceItem in Items)
        {
            if (resourceItem.Name == item.Name)
            {
                resourceItem.Amount = Mathf.Max(0, resourceItem.Amount - item.Amount);
                OnResourceModified(item.Name);
                return;
            }
        }
    }

    public int GetResourceAmount(string resourceType)
    {
        foreach (ResourceItem item in Items)
        {
            if (item.Name == resourceType)
            {
                return item.Amount;
            }
        }

        return 0;
    }
    #endregion

    #region HEROES

    public void SelectHero(string newHero)
    {
        SelectedHero = newHero;
    }

    public void SelectItem(string newItem)
    {
        if (SelectedItems.Count < 2)
        {
            SelectedItems.Add(newItem);
            OnBattleItemSelected?.Invoke();
        }
    }

    public void DeselectItem(string item)
    {
        SelectedItems.Remove(item);
        OnBattleItemDeSelected?.Invoke();
    }

    public void AddHero(ResourceItem item)
    {
        foreach (OwnedHero hero in Heroes)
        {
            if (hero.Id == item.Name)
            {
                hero.Level++;
                OnHeroModified?.Invoke(hero.Id);
                return;
            }
        }
        Heroes.Add(new OwnedHero(item.Name, "Hero", 1));
        OnHeroAdded?.Invoke();
    }

    public List<OwnedHero> GetOwnedHeroes()
    {
        return Heroes;
    }

    public string GetSelectedHero()
    {
        return SelectedHero;
    }

    #endregion

    #region BATTLEITEMS

    public void AddBattleItem(ResourceItem item)
    {
        foreach (OwnedBattleItem battleItem in BattleItems)
        {
            if (battleItem.Id == item.Name)
            {
                battleItem.Amount++;
                OnBattleItemModified?.Invoke();
                return;
            }
        }
        BattleItems.Add(new OwnedBattleItem(item.Name, "BattleItem", 1));
        OnBattleItemAdded?.Invoke();
    }

    public void RemoveBattleItem(string itemName)
    {
        foreach (OwnedBattleItem battleItem in BattleItems.ToList())
        {
            if (battleItem.Id == itemName)
            {
                if (battleItem.Amount - 1 > 0)
                {
                    battleItem.Amount--;
                    OnBattleItemModified?.Invoke();
                    return;
                }
                BattleItems.Remove(battleItem);
                OnBattleItemAdded?.Invoke();
            }
        }
    }

    public List<OwnedBattleItem> GetOwnedBattleItems()
    {
        return BattleItems;
    }

    public int GetBattleItemAmount(string itemName)
    {
        foreach (OwnedBattleItem item in BattleItems)
        {
            if (item.Id == itemName) return item.Amount;
        }
        return 0;
    }

    public List<string> GetSelectedItems()
    {
        return SelectedItems;
    }

    public List<OwnedBattleItem> GetSelectedItems2()
    {
        List<OwnedBattleItem> list = new();
        foreach (string selectedItem in SelectedItems)
        {
            foreach (OwnedBattleItem item in BattleItems)
            {
                if (selectedItem == item.Id) list.Add(item);
            }
        }
        return list;
    }

    #endregion

    #region LEVELS

    public int GetLevelsPassed()
    {
        return levelsPassed;
    }

    public int GetCurrentSelectedLevel()
    {
        return currentSelectedLevel;
    }

    public void SetCurrentSelectedLevel(int nextLevel)
    {
        currentSelectedLevel = nextLevel;
    }

    #endregion

    #region SAVE/LOAD
    public void Save()
    {
        string jsonObject = JsonUtility.ToJson(this);

        File.WriteAllText(path, jsonObject);
    }

    public void Load()
    {
        string readFile = File.Exists(path) ? File.ReadAllText(path) : "{}";

        JsonUtility.FromJsonOverwrite(readFile, this);
    }
    #endregion
}

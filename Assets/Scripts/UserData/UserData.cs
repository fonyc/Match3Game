using Shop.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class UserData
{
    //Stores Gold, Gems, and potions (qty)
    [SerializeField] private List<ResourceItem> Items = new();

    //Stores Owned Heroes
    [SerializeField] private List<OwnedHero> Heroes = new();

    [SerializeField] private List<OwnedBattleItem> BattleItems = new();

    private string path = Application.persistentDataPath + "/userData.data";
    public event Action<string> OnResourceModified = resource => { };
    public event Action<string> OnHeroModified = hero => { };
    public event Action OnHeroAdded;
    public event Action<string> OnBattleItemModified = hero => { };
    public event Action OnBattleItemAdded;

    #region RESOURCES

    public void AddPrimaryResources(string resource)
    {
        foreach (ResourceItem resourceItem in Items)
        {
            if (resourceItem.Name == resource)
            {
                resourceItem.Amount += 100;
                OnResourceModified(resource);
                return;
            }
        }
        Items.Add(new ResourceItem { Name = resource, Type = "Resources", Amount = 100 });
        OnResourceModified(resource);
    }

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

    public void AddHero(ResourceItem item)
    {
        foreach (OwnedHero hero in Heroes)
        {
            if (hero.Name == item.Name)
            {
                //Hero is found and modified
                hero.Level++;
                OnHeroModified?.Invoke(hero.Name);
                return;
            }
        }
        //New hero added to collection
        Heroes.Add(new OwnedHero(item.Name, 1));
        OnHeroAdded?.Invoke();
    }

    public List<OwnedHero> GetOwnedHeroes()
    {
        return Heroes;
    }

    #endregion

    #region BATTLEITEMS

    public void AddBattleItem(ResourceItem item)
    {
        foreach (OwnedBattleItem battleItem in BattleItems)
        {
            if (battleItem.Name == item.Name)
            {
                battleItem.Amount++;
                OnBattleItemModified?.Invoke(battleItem.Name);
                return;
            }
        }
        BattleItems.Add(new OwnedBattleItem(item.Name, 1));
        OnBattleItemAdded?.Invoke();
    }

    public void RemoveBattleItem(string itemName)
    {
        foreach (OwnedBattleItem battleItem in BattleItems)
        {
            if (battleItem.Name == itemName)
            {
                if(battleItem.Amount - 1 > 0)
                {
                    battleItem.Amount--;
                    OnBattleItemModified?.Invoke(battleItem.Name);
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

    #endregion

    #region SAVE/LOAD
    public void Save()
    {
        string jsonObject = JsonUtility.ToJson(this);

        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        fs.Write(Encoding.UTF8.GetBytes(jsonObject), 0, Encoding.UTF8.GetByteCount(jsonObject));
        fs.Close();
    }

    public void Load()
    {
        string readFile = File.Exists(path) ? File.ReadAllText(path) : "{}";
        
        JsonUtility.FromJsonOverwrite(readFile, this);
    }
    #endregion
}

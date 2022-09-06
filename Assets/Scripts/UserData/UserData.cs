using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using static UnityEditor.Progress;

public class UserData
{
    //Stores Gold, Gems, potions and hero tokens (qty)
    [SerializeField] private List<ResourceItem> Items = new();

    //Stores Owned Heroes
    [SerializeField] private List<OwnedHero> Heroes = new();

    private string path = Application.persistentDataPath + "/userData.data";
    public event Action<string> OnResourceModified = resource => { };
    public event Action<string> OnHeroModified = hero => { };

    #region RESOURCES

    public void AddGold()
    {
        foreach (ResourceItem resourceItem in Items)
        {
            if (resourceItem.Type == "Gold")
            {
                resourceItem.Amount += 100;
                OnResourceModified("Gold");
                return;
            }
        }
        Items.Add(new ResourceItem { Type = "Gold", Amount = 100 });
        OnResourceModified("Gold");
    }

    public void AddResource(ResourceItem item)
    {
        foreach (ResourceItem resourceItem in Items)
        {
            if (resourceItem.Type == item.Type)
            {
                resourceItem.Amount += item.Amount;
                OnResourceModified(item.Type);
                return;
            }
        }

        Items.Add(new ResourceItem { Type = item.Type, Amount = item.Amount });
        OnResourceModified(item.Type);
    }

    public void RemoveResource(ResourceItem item)
    {
        foreach (ResourceItem resourceItem in Items)
        {
            if (resourceItem.Type == item.Type)
            {
                resourceItem.Amount = Mathf.Max(0, resourceItem.Amount - item.Amount);
                OnResourceModified(item.Type);
                return;
            }
        }
    }

    public int GetResourceAmount(string resourceType)
    {
        foreach (ResourceItem item in Items)
        {
            if (item.Type == resourceType)
            {
                return item.Amount;
            }
        }

        return 0;
    }
    #endregion

    #region HEROES

    public void AddHero(OwnedHero newHero)
    {
        bool found = false;
        foreach (OwnedHero hero in Heroes)
        {
            if (hero.Name == newHero.Name)
            {
                hero.Level++;
                found = true;
            }
        }
        if (!found) Heroes.Add(newHero);
    }


    public List<OwnedHero> GetOwnedHeroList()
    {
        return Heroes;
    }

    #endregion

    //public void Save() => PlayerPrefs.SetString("USERDATA2", JsonUtility.ToJson(this));

    //public void Load() => JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("USERDATA2", "{}"), this);


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
}

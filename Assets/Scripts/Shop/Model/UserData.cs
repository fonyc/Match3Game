using System;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    [SerializeField]
    private List<ResourceItem> Items = new();

    //private List<Hero> HeroCollection = new();

    public event Action<string> OnResourceModified = resource => { };

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

    public void Save() => PlayerPrefs.SetString("USERDATA", JsonUtility.ToJson(this));
    public void Load() => JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("USERDATA", "{}"), this);
}

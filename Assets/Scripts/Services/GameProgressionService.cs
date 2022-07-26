﻿using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class GameProgressionService : IService
{
    [SerializeField] private List<ResourceItem> Items = new();

    [SerializeField] private List<OwnedHero> Heroes = new();

    [SerializeField] private List<OwnedBattleItem> BattleItems = new();

    [SerializeField] private string SelectedHero = null;

    [SerializeField] private List<string> SelectedItems = new();

    [SerializeField] private int levelsPassed;

    [SerializeField] private int currentSelectedLevel;

    private IGameProgressionProvider _progressionProvider;

    public void Initialize(GameConfigService gameConfig, IGameProgressionProvider progressionProvider)
    {
        _progressionProvider = progressionProvider;
        Load(gameConfig);
    }

    #region EVENTS
    public event Action<string> OnResourceModified = resource => { };
    public event Action<string> OnHeroModified = hero => { };
    public event Action OnHeroAdded;
    public event Action OnBattleItemModified;
    public event Action OnBattleItemAdded;
    public event Action OnBattleItemSelected;
    public event Action OnBattleItemDeSelected;
    public event Action OnHeroSelected;
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
                Save();
                return;
            }
        }

        Items.Add(new ResourceItem { Name = item.Name, Type = item.Type, Amount = item.Amount });
        OnResourceModified(item.Name);
        Save();
    }

    public void RemoveResource(ResourceItem item)
    {
        foreach (ResourceItem resourceItem in Items)
        {
            if (resourceItem.Name == item.Name)
            {
                resourceItem.Amount = Mathf.Max(0, resourceItem.Amount - item.Amount);
                OnResourceModified(item.Name);
                Save();
                return;
            }
        }
        Save();
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
        OnHeroSelected?.Invoke();
        Save();
    }

    public void AddHero(ResourceItem item)
    {
        foreach (OwnedHero hero in Heroes)
        {
            if (hero.Id == item.Name)
            {
                hero.Level++;
                OnHeroModified?.Invoke(hero.Id);
                Save();
                return;
            }
        }
        Heroes.Add(new OwnedHero(item.Name, "Hero", 1));
        OnHeroAdded?.Invoke();
        Save();
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

    public void SelectItem(string newItem)
    {
        if (SelectedItems.Count < 2)
        {
            SelectedItems.Add(newItem);
            OnBattleItemSelected?.Invoke();
        }
        Save();
    }

    public void DeselectItem(string item)
    {
        SelectedItems.Remove(item);
        OnBattleItemDeSelected?.Invoke();
        Save();
    }

    public void AddBattleItem(ResourceItem item)
    {
        foreach (OwnedBattleItem battleItem in BattleItems)
        {
            if (battleItem.Id == item.Name)
            {
                battleItem.Amount++;
                OnBattleItemModified?.Invoke();
                Save();
                return;
            }
        }
        BattleItems.Add(new OwnedBattleItem(item.Name, "BattleItem", 1));
        OnBattleItemAdded?.Invoke();
        Save();
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
                    Save();
                    return;
                }
                BattleItems.Remove(battleItem);
                SelectedItems.Remove(battleItem.Id);
                OnBattleItemAdded?.Invoke();
            }
        }
        Save();
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
        Save();
    }

    public void AddLevelUnlocked()
    {
        levelsPassed++;
        Save();
    }

    #endregion

    #region SAVE/LOAD
    private void Load(GameConfigService config)
    {
        string data = _progressionProvider.Load();
        if (string.IsNullOrEmpty(data))
        {
            List<ResourceItem> initialResources = config.initialResources;
            foreach(ResourceItem resource in initialResources)
            {
                if(resource.Type == "Hero") AddHero(resource);
                else if(resource.Type == "Resources") AddResource(resource);
                else AddBattleItem(resource);
            }
            Save();
        }
        else
        {
            JsonUtility.FromJsonOverwrite(data, this);
        }
    }

    private void Save()
    {
        _progressionProvider.Save(JsonUtility.ToJson(this));
    }
    #endregion

    public void Clear()
    {

    }
}
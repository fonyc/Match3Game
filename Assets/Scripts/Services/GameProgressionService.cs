using System.Collections.Generic;
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
                SelectedItems.Remove(battleItem.Id);
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

    public void AddLevelUnlocked()
    {
        levelsPassed++;
    }

    #endregion

    #region SAVE/LOAD
    private void Load(GameConfigService config)
    {
        string data = _progressionProvider.Load();
        if (string.IsNullOrEmpty(data))
        {
            //Gems = config.InitialGems;
            //_gold = config.InitialGold;
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
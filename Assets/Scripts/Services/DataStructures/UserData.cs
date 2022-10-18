using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    [SerializeField] private List<ResourceItem> Items = new();

    [SerializeField] private List<OwnedHero> Heroes = new();

    [SerializeField] private List<OwnedBattleItem> BattleItems = new();

    [SerializeField] private string SelectedHero = null;

    [SerializeField] private List<string> SelectedItems = new();

    [SerializeField] private int levelsPassed;

    [SerializeField] private int currentSelectedLevel;

    public int GetHeroNumber()
    {
        if (Heroes == null) return 0;
        return Heroes.Count;
    }

    public int GetGems()
    {
        if (Items == null) return 0;
        foreach (ResourceItem item in Items)
        {
            if (item.Name == "Gems") return item.Amount;
        }
        return 0;
    }
}
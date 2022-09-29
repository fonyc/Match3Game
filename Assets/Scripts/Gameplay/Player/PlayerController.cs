﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: IDisposable
{
    private UserData _userData;
    private PlayerModel _playerModel;
    private ItemController _itemController;

    public event Action<int, int> OnHPChanged = delegate (int amount, int max) { };
    public event Action OnATKChanged = delegate () { };
    public event Action OnDEFChanged = delegate () { };

    public PlayerController(UserData userData, ItemController itemController)
    {
        _itemController = itemController;
        _itemController._onHPItemConsumed += ChangeHP;
        _itemController._onATKItemConsumed += ChangeATK;
        _itemController._onDEFItemConsumed += ChangeDEF;

        _userData = userData;
        _playerModel = new PlayerModel();
    }

    public void Initialize()
    {
        LoadSelectedHero();
    }

    public HeroItemModel GetHero()
    {
        return _playerModel.hero;
    }

    public HeroStats GetCurrentStats()
    {
        return _playerModel.currentHeroStats;
    }

    private void LoadSelectedHero()
    {
        HeroModel allHeroesModel = JsonUtility.FromJson<HeroModel>(Resources.Load<TextAsset>("HeroModel").text);
        _playerModel.hero = GetHeroData(allHeroesModel);
        _playerModel.currentHeroStats = _playerModel.hero.Stats;
    }

    private HeroItemModel GetHeroData(HeroModel allHeroesModel)
    {
        foreach (HeroItemModel hero in allHeroesModel.Heroes)
        {
            if (hero.Id == _userData.GetSelectedHero()) return hero;
        }
        return null;
    }

    #region EVENTS
    public void ChangeHP(int amount)
    {
        int maxHP = _playerModel.hero.Stats.HP;
        int currentHeroHP = _playerModel.currentHeroStats.HP;
        _playerModel.currentHeroStats.HP = Mathf.Clamp(currentHeroHP + amount, 0, maxHP);
        OnHPChanged.Invoke(_playerModel.currentHeroStats.HP, _playerModel.hero.Stats.HP);
        //if(CheckPlayerDeath())
    }

    public void ChangeATK(int amount)
    {
        _playerModel.currentHeroStats.ATK += amount;
        OnATKChanged.Invoke();
    }

    public void ChangeDEF(int amount)
    {
        _playerModel.currentHeroStats.DEF += amount;
        OnDEFChanged.Invoke();
    }
    #endregion

    private bool CheckPlayerDeath()
    {
        return _playerModel.currentHeroStats.HP == 0;
    }

    public void Dispose()
    {
        _itemController._onHPItemConsumed -= ChangeHP;
        _itemController._onATKItemConsumed -= ChangeATK;
        _itemController._onDEFItemConsumed -= ChangeDEF;
    }
}
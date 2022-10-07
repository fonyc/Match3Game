using Board.Controller;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : IDisposable
{
    private UserData _userData;
    private PlayerModel _playerModel;
    private ItemController _itemController;
    private CombatController _combatController;

    public event Action<int, int> OnHPChanged = delegate (int amount, int max) { };
    public event Action OnATKChanged = delegate () { };
    public event Action OnDEFChanged = delegate () { };
    StatIntIntArgument_Event _onPlayerAttackPerformed;

    public PlayerController(UserData userData, ItemController itemController, CombatController combatController,
        StatIntIntArgument_Event OnPlayerAttackPerformed)
    {

        _onPlayerAttackPerformed = OnPlayerAttackPerformed;
        _combatController = combatController;
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

    public Stats GetCurrentStats()
    {
        return _playerModel.currentHeroStats;
    }

    private void LoadSelectedHero()
    {
        List<HeroItemModel> allHeroesModel = ServiceLocator.GetService<GameConfigService>().HeroModel;
        _playerModel.hero = GetHeroData(allHeroesModel);
       
        Stats stats = _playerModel.hero.Stats;
        _playerModel.currentHeroStats = new Stats(stats.ATK, stats.DEF, stats.HP, stats.Progression);
    }

    private HeroItemModel GetHeroData(List<HeroItemModel> allHeroesModel)
    {
        foreach (HeroItemModel hero in allHeroesModel)
        {
            if (hero.Id == _userData.GetSelectedHero()) return hero;
        }
        return null;
    }

    public void RecieveAttack(Stats enemyStats, int hits, int color)
    {
        int dmg = _combatController.RecieveAttack(enemyStats.ATK, _playerModel.currentHeroStats.DEF, 1, color, _playerModel.hero.Color);

        int currentHP = _playerModel.currentHeroStats.HP;
        _playerModel.currentHeroStats.HP = currentHP - dmg <= 0 ? 0 : currentHP - dmg;

        OnHPChanged?.Invoke(_playerModel.currentHeroStats.HP, _playerModel.hero.Stats.HP);
    }

    #region EVENTS

    public void AttackEnemy(int hits, int color)
    {
        _onPlayerAttackPerformed.TriggerEvents(_playerModel.hero.Stats, hits, color);
    }

    public void ChangeHP(int amount)
    {
        int maxHP = _playerModel.hero.Stats.HP;
        int currentHeroHP = _playerModel.currentHeroStats.HP;
        _playerModel.currentHeroStats.HP = Mathf.Clamp(currentHeroHP + amount, 0, maxHP);
        OnHPChanged.Invoke(_playerModel.currentHeroStats.HP, _playerModel.hero.Stats.HP);

        if(CheckPlayerDeath())
        {
            Debug.Log("players dead");
        }
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
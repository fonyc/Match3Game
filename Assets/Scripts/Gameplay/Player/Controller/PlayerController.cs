using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : IDisposable
{
    private UserData _userData;
    private PlayerModel _playerModel;
    private ItemController _itemController;
    private CombatController _combatController;
    private GameConfigService _gameConfigService;

    public event Action<int, int> OnHPChanged = delegate (int amount, int max) { };
    public event Action OnATKChanged = delegate () { };
    public event Action OnDEFChanged = delegate () { };
    private StatTripleIntArgument_Event _onPlayerAttackPerformed;
    private NoArgument_Event _onPlayerDied;
    private NoArgument_Event _onPlayerRecievedDamage;
    private MatchReport _matchReport;

    public PlayerController(UserData userData, ItemController itemController, GameConfigService gameConfigService,
        CombatController combatController, StatTripleIntArgument_Event OnPlayerAttackPerformed, NoArgument_Event OnPlayerDied,
        NoArgument_Event OnPlayerRecievedDamage, MatchReport matchReport)
    {
        _matchReport = matchReport;
        _gameConfigService = gameConfigService;
        _onPlayerRecievedDamage = OnPlayerRecievedDamage;
        _onPlayerDied = OnPlayerDied;
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
        List<HeroItemModel> allHeroesModel = _gameConfigService.HeroModel;
        _playerModel.hero = GetHeroData(allHeroesModel);

        Stats stats = _playerModel.hero.Stats;
        _playerModel.currentHeroStats = new Stats(stats.ATK, stats.DEF, stats.HP, stats.ManaPerHit, stats.Progression);
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
        _matchReport.damageRecieved = dmg;
        ChangeHP(-dmg);

        if (CheckPlayerDeath()) _onPlayerDied.TriggerEvents();
        else _onPlayerRecievedDamage.TriggerEvents();
    }

    #region EVENTS

    public void AttackEnemy(int hits, int color, int columns)
    {
        _onPlayerAttackPerformed.TriggerEvents(_playerModel.hero.Stats, hits, color, columns);
    }

    public void ChangeHP(int amount)
    {
        int maxHP = _playerModel.hero.Stats.HP;
        int currentHP = _playerModel.currentHeroStats.HP;

        _playerModel.currentHeroStats.HP = currentHP + amount <= 0 ? 0 : currentHP + amount;
        OnHPChanged?.Invoke(_playerModel.currentHeroStats.HP, maxHP);
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
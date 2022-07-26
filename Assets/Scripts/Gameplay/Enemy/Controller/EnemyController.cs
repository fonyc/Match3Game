using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController
{
    public EnemyModel Model;
    //public Stats CurrentEnemyStats;

    private GameProgressionService _gameProgression;
    private CombatController _combatController;

    NoArgument_Event _onEnemyDied;
    StatIntIntArgument_Event _onEnemyAttacks;
    public event Action<int, int> OnHPChanged = delegate (int hp, int max) { };
    private GameConfigService _gameConfig;
    private MatchReport _matchReport;

    public EnemyController(GameProgressionService gameProgression, CombatController combatController, StatIntIntArgument_Event OnEnemyAttacks,
        NoArgument_Event OnEnemyDied, GameConfigService gameConfigService, MatchReport matchReport)
    {
        _matchReport = matchReport;
        _gameConfig = gameConfigService;
        _onEnemyDied = OnEnemyDied;
        Model = new EnemyModel();
        _combatController = combatController;
        _gameProgression = gameProgression;
        _onEnemyAttacks = OnEnemyAttacks;
    }

    public void AttackPlayer()
    {
        _onEnemyAttacks.TriggerEvents(Model.CurrentEnemyStats, 1, Model.Enemy.Color);
    }

    public void Initialize()
    {
        Load();
    }

    public void RecieveDamageFromPlayer(Stats heroStats, int hits, int colorAttack, int columns)
    {
        int dmg = _combatController.RecieveAttack(heroStats.ATK, Model.CurrentEnemyStats.DEF, hits,
            colorAttack, Model.Enemy.Color) + columns * _gameConfig.columnBonus;

        _matchReport.damageDealt += dmg;

        Model.CurrentEnemyStats.HP = Model.CurrentEnemyStats.HP - dmg <= 0 ? 0 : Model.CurrentEnemyStats.HP - dmg;

        OnHPChanged?.Invoke(Model.CurrentEnemyStats.HP, Model.Enemy.Stats.HP);

        if (CheckDeath(Model.CurrentEnemyStats.HP))
        {
            _onEnemyDied.TriggerEvents();
        }
    }

    private bool CheckDeath(int hp)
    {
        return hp <= 0;
    }

    private void Load()
    {
        Model = new EnemyModel();

        int currentLevel = _gameProgression.GetCurrentSelectedLevel();
        List<LevelModelItem> allLevels = _gameConfig.LevelsModel;
        string enemyId = GetEnemyIdFromCurrentLevel(currentLevel, allLevels);
        List<Enemy> allEnemies = _gameConfig.EnemyModel;
        Model.Enemy = GetEnemy(enemyId, allEnemies);

        Model.CurrentEnemyStats = new Stats(Model.Enemy.Stats.ATK, Model.Enemy.Stats.DEF,
            Model.Enemy.Stats.HP, Model.Enemy.Stats.ManaPerHit, Model.Enemy.Stats.Progression);
    }

    private Enemy GetEnemy(string Id, List<Enemy> enemyModel)
    {
        foreach (Enemy enemy in enemyModel)
        {
            if (enemy.Id == Id) return enemy;
        }
        return null;
    }

    private string GetEnemyIdFromCurrentLevel(int currentLevel, List<LevelModelItem> allLevels)
    {
        foreach (LevelModelItem level in allLevels)
        {
            if (level.Level == currentLevel) return level.Enemy;
        }
        return null;
    }

    public Stats GetEnemyStats()
    {
        return Model.Enemy.Stats;
    }
}

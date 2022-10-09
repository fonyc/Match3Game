using System;
using System.Collections.Generic;

public class EnemyController
{
    public EnemyModel Model;
    //public Stats CurrentEnemyStats;

    private UserData _userData;
    private CombatController _combatController;

    NoArgument_Event _onEnemyDied;
    StatIntIntArgument_Event _onEnemyAttacks;
    public event Action<int, int> OnHPChanged = delegate (int hp, int max) { };
    private GameConfigService _gameConfig;

    public EnemyController(UserData userData, CombatController combatController, StatIntIntArgument_Event OnEnemyAttacks,
        NoArgument_Event OnEnemyDied, GameConfigService gameConfigService)
    {
        _gameConfig = gameConfigService;
        _onEnemyDied = OnEnemyDied;
        Model = new EnemyModel();
        _combatController = combatController;
        _userData = userData;
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

    public void RecieveDamageFromPlayer(Stats heroStats, int hits, int colorAttack)
    {
        int dmg = _combatController.RecieveAttack(heroStats.ATK, Model.CurrentEnemyStats.DEF, hits, colorAttack, Model.Enemy.Color);

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

        int currentLevel = _userData.GetCurrentSelectedLevel();
        List<LevelModelItem> allLevels = _gameConfig.LevelsModel;
        string enemyId = GetEnemyIdFromCurrentLevel(currentLevel, allLevels);

        List<Enemy> allEnemies = _gameConfig.EnemyModel;
        Model.Enemy = GetEnemy(enemyId, allEnemies);

        Model.CurrentEnemyStats = new Stats(Model.Enemy.Stats.ATK, Model.Enemy.Stats.DEF, Model.Enemy.Stats.HP, Model.Enemy.Stats.Progression);
        Model.CurrentEnemyStats.HP = 1;
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

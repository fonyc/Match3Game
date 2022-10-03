using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController
{
    public EnemyModel Model;
    //public Stats CurrentEnemyStats;

    private UserData _userData;
    private CombatController _combatController;

    public event Action<int> OnHPChanged = delegate (int hp) { };

    public EnemyController(UserData userData, CombatController combatController)
    {
        Model = new EnemyModel();
        _combatController = combatController;
        _userData = userData;
    }

    public void Initialize()
    {
        Load();
    }

    public void RecieveDamageFromPlayer(Stats heroStats, int hits, int colorAttack)
    {
        int dmg = _combatController.RecieveAttack(heroStats.ATK, Model.CurrentEnemyStats.DEF, hits, colorAttack, Model.Enemy.Color);

        Model.CurrentEnemyStats.HP = Model.CurrentEnemyStats.HP - dmg <= 0 ? 0 : Model.CurrentEnemyStats.HP - dmg;

        OnHPChanged?.Invoke(Model.CurrentEnemyStats.HP);
    }

    private void Load()
    {
        Model = new EnemyModel();

        int currentLevel = _userData.GetCurrentSelectedLevel();
        //LevelModel allLevels = JsonUtility.FromJson<LevelModel>(Resources.Load<TextAsset>("LevelsModel").text);
        List<LevelModelItem> allLevels = ServiceLocator.GetService<GameConfigService>().LevelsModel;
        string enemyId = GetEnemyIdFromCurrentLevel(currentLevel, allLevels);

        //EnemiesModel allEnemies = JsonUtility.FromJson<EnemiesModel>(Resources.Load<TextAsset>("EnemyModel").text);
        List<Enemy> allEnemies = ServiceLocator.GetService<GameConfigService>().EnemyModel;
        Model.Enemy = GetEnemy(enemyId, allEnemies);
        //Model.CurrentEnemyStats = Model.Enemy.Stats;
        Model.CurrentEnemyStats = new Stats(Model.Enemy.Stats.ATK, Model.Enemy.Stats.DEF, Model.Enemy.Stats.HP, Model.Enemy.Stats.Progression);
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
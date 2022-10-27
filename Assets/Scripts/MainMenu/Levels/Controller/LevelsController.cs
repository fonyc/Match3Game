using System.Collections.Generic;
using UnityEngine;

public class LevelsController 
{
    public LevelModel LevelModel;
    public EnemiesModel EnemyModel;
    private GameProgressionService _gameProgression;
    private SceneLoader _sceneLoader;
    private GameConfigService _gameConfig;

    public LevelsController(GameProgressionService userData, SceneLoader sceneLoader, GameConfigService gameConfig)
    {
        _gameConfig = gameConfig;
        _gameProgression = userData;
        _sceneLoader = sceneLoader;
    }

    public void Initialize()
    {
        Load();
    }

    public void ChangeGameplayScene(int level)
    {
        if (string.IsNullOrEmpty(_gameProgression.GetSelectedHero())) return;

        _gameProgression.SetCurrentSelectedLevel(level);
        _sceneLoader.ChangeScene(2);
    }

    public void Load()
    {
        LevelModel = new LevelModel();

        LevelModel.Levels = _gameConfig.LevelsModel;
    }
}

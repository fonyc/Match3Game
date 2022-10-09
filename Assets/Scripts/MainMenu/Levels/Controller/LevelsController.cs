using System.Collections.Generic;
using UnityEngine;

public class LevelsController 
{
    public LevelModel LevelModel;
    public EnemiesModel EnemyModel;
    private UserData _userData;
    private SceneLoader _sceneLoader;
    private GameConfigService _gameConfig;

    public LevelsController(UserData userData, SceneLoader sceneLoader, GameConfigService gameConfig)
    {
        _gameConfig = gameConfig;
        _userData = userData;
        _sceneLoader = sceneLoader;
    }

    public void Initialize()
    {
        Load();
    }

    public void ChangeGameplayScene(int level)
    {
        _userData.SetCurrentSelectedLevel(level);
        _userData.Save();

        _sceneLoader.ChangeScene(2);
    }

    public void Load()
    {
        LevelModel = new LevelModel();

        //LevelModel = JsonUtility.FromJson<LevelModel>(Resources.Load<TextAsset>("LevelsModel").text);
        LevelModel.Levels = _gameConfig.LevelsModel;
    }
}

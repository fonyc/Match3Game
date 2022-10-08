using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class MatchController 
{
    public LevelModelItem levelModel;

    private UserData _userData;
    private GameConfigService _gameConfigService;
    private SceneLoader _sceneLoader;

    public MatchController(GameConfigService gameConfigService, UserData userData, SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
        _userData = userData;
        _gameConfigService = gameConfigService;
    }

    public void Initialize()
    {
        Load();
    }

    public void GrantRewards()
    {
        //Resource Rewards
        foreach(ResourceItem reward in levelModel.Rewards)
        {
            _userData.AddResource(reward);
            _userData.Save();
        }

        //Level Reward
        int currentUnlockedLevel = _userData.GetLevelsPassed();
        int levelPlayed = _userData.GetCurrentSelectedLevel();
        int levels = _gameConfigService.LevelsModel.Count;
        if(levelPlayed == currentUnlockedLevel && currentUnlockedLevel < levels)
        {
            _userData.AddLevelUnlocked();
            _userData.Save();
        } 
    }

    public void GoToMainMenu()
    {
        _sceneLoader.ChangeScene(1);
    }

    private void Load()
    {
        levelModel = new LevelModelItem();
        List<LevelModelItem> levels = _gameConfigService.LevelsModel;
        levelModel = GetLevel(_userData.GetCurrentSelectedLevel(), levels);
    }

    private LevelModelItem GetLevel(int currentLevel, List<LevelModelItem> levels)
    {
        return levels.Find(level => level.Level == currentLevel);
    }
}

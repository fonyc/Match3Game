using System;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class MatchController
{
    public LevelModelItem levelModel;

    public event Action OnAddWatched = delegate () { };

    private UserData _userData;
    private GameConfigService _gameConfigService;
    private SceneLoader _sceneLoader;
    private AdsGameService _adsGameService;
    private AnalyticsGameService _analytics;

    public MatchController(GameConfigService gameConfigService, UserData userData, SceneLoader sceneLoader,
        AdsGameService AdsGameService, AnalyticsGameService Analytics)
    {
        _analytics = Analytics;
        _adsGameService = AdsGameService;
        _sceneLoader = sceneLoader;
        _userData = userData;
        _gameConfigService = gameConfigService;
    }

    public void Initialize()
    {
        Load();
        _analytics.SendEvent("levelStarted", new Dictionary<string, object> { ["LevelNumber"] = levelModel.Level });
    }

    public void GrantRewards()
    {
        _analytics.SendEvent("levelEnded", new Dictionary<string, object> { ["LevelNumber"] = levelModel.Level });

        //Resource Rewards
        foreach (ResourceItem reward in levelModel.Rewards)
        {
            _userData.AddResource(reward);
            _userData.Save();
        }

        //Level Reward
        int currentUnlockedLevel = _userData.GetLevelsPassed();
        int levelPlayed = _userData.GetCurrentSelectedLevel();
        int levels = _gameConfigService.LevelsModel.Count;

        if (levelPlayed == currentUnlockedLevel && currentUnlockedLevel < levels)
        {
            _userData.AddLevelUnlocked();
            _userData.Save();
        }
    }

    public async void GrantAddReward()
    {
        if (await _adsGameService.ShowAd())
        {
            GrantRewards();
            OnAddWatched?.Invoke();
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

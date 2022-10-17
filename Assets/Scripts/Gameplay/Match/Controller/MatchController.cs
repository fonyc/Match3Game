using System;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class MatchController
{
    public LevelModelItem levelModel;

    public event Action OnAddWatched = delegate () { };

    private GameProgressionService _gameProgression;
    private GameConfigService _gameConfigService;
    private SceneLoader _sceneLoader;
    private AdsGameService _adsGameService;
    private AnalyticsGameService _analytics;

    public MatchController(GameConfigService gameConfigService, GameProgressionService gameProgression, SceneLoader sceneLoader,
        AdsGameService AdsGameService, AnalyticsGameService Analytics)
    {
        _analytics = Analytics;
        _adsGameService = AdsGameService;
        _sceneLoader = sceneLoader;
        _gameProgression = gameProgression;
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
            _gameProgression.AddResource(reward);
            //_gameProgression.Save();
        }

        //Level Reward
        int currentUnlockedLevel = _gameProgression.GetLevelsPassed();
        int levelPlayed = _gameProgression.GetCurrentSelectedLevel();
        int levels = _gameConfigService.LevelsModel.Count;

        if (levelPlayed == currentUnlockedLevel && currentUnlockedLevel < levels)
        {
            _gameProgression.AddLevelUnlocked();
            //_gameProgression.Save();
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
        levelModel = GetLevel(_gameProgression.GetCurrentSelectedLevel(), levels);
    }

    private LevelModelItem GetLevel(int currentLevel, List<LevelModelItem> levels)
    {
        return levels.Find(level => level.Level == currentLevel);
    }
}

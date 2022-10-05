using System.Collections;
using System.Collections.Generic;

public class MatchController 
{
    public LevelModelItem levelModel;

    private UserData _userData;
    private GameConfigService _gameConfigService;

    public MatchController(GameConfigService gameConfigService, UserData userData)
    {
        _userData = userData;
        _gameConfigService = gameConfigService;
    }

    public void Initialize()
    {
        Load();
    }

    public void GrantRewards()
    {
        foreach(ResourceItem reward in levelModel.Rewards)
        {
            _userData.AddResource(reward);
        }
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

using Shop.Model;
using UnityEngine;

public class HeroesController 
{
    public HeroModel Model { get; private set; }

    public UserData UserData { get; private set; }

    private GameConfigService _gameConfigService;

    public HeroesController(UserData userData, GameConfigService GameConfigService)
    {
        _gameConfigService = GameConfigService;
        UserData = userData;
    }

    public void Initialize()
    {
        Load();
    }

    private void Load()
    {
        Model = new HeroModel();
        Model.Heroes = _gameConfigService.HeroModel;
    }
}

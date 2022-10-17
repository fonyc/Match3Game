using Shop.Model;
using UnityEngine;

public class HeroesController 
{
    public HeroModel Model { get; private set; }

    public GameConfigService _gameConfigService { get; private set; }

    public HeroesController(GameConfigService GameConfigService)
    {
        _gameConfigService = GameConfigService;
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

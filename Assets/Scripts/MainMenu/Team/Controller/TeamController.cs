using System.Collections.Generic;
using System.Linq;

public class TeamController 
{
    public HeroModel heroModel;

    public BattleItemsModel battleItemModel;

    private GameProgressionService _gameProgression;
    private GameConfigService _gameConfig;

    public TeamController(GameProgressionService userData, GameConfigService gameConfigService)
    {
        _gameConfig = gameConfigService;
        _gameProgression = userData;
    }

    public void SelectHero(string heroName)
    {
        _gameProgression.SelectHero(heroName);
    }

    public void SelectItem(string itemName)
    {
        List<string> selectedItems = _gameProgression.GetSelectedItems();

        if (selectedItems.Contains(itemName)) _gameProgression.DeselectItem(itemName);
        else if (selectedItems.Count < 2) _gameProgression.SelectItem(itemName);
        else 
        {
            _gameProgression.DeselectItem(selectedItems.Last());
            _gameProgression.SelectItem(itemName);
        }
    }

    public void Initialize()
    {
        Load();
    }

    private void Load()
    {
        heroModel = new HeroModel();
        battleItemModel = new BattleItemsModel();

        heroModel.Heroes = _gameConfig.HeroModel;
        battleItemModel.BattleItems = _gameConfig.BattleItemsModel;
    }
}

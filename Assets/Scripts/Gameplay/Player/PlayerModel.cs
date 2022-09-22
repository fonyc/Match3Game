using System.Collections.Generic;

public class PlayerModel
{
    public HeroItemModel hero;
    public List<BattleItemModel> itemStats = new();
    public List<OwnedBattleItem> ownedItems = new();
    public HeroStats currentHeroStats;

    public PlayerModel()
    {

    }
}
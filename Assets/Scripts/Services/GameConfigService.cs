using Shop.Model;
using System.Collections.Generic;

public class GameConfigService : IService
{
    public List<ShopItemModel> ShopOffers { get; private set; }
    public List<HeroItemModel> HeroModel { get; private set; }
    public List<Enemy> EnemyModel { get; private set; }
    public List<LevelModelItem> LevelsModel { get; private set; }
    public List<BattleItemModel> BattleItemsModel { get; private set; }
    public List<EmblemInteraction> VulnerabilitiesModel { get; private set; }
    public List<SkillItemModel> SkillModel { get; private set; }
    public int dangerThreshold { get; private set; }
    public void Initialize(RemoteConfigGameService dataProvider)
    {
        ShopOffers = dataProvider.Get("ShopModel", new List<ShopItemModel>());
        HeroModel = dataProvider.Get("HeroModel", new List<HeroItemModel>());
        LevelsModel = dataProvider.Get("LevelsModel", new List<LevelModelItem>());
        EnemyModel = dataProvider.Get("EnemyModel", new List<Enemy>());
        BattleItemsModel = dataProvider.Get("BattleItemsModel", new List<BattleItemModel>());
        SkillModel = dataProvider.Get("SkillModel", new List<SkillItemModel>());
        VulnerabilitiesModel = dataProvider.Get("VulnerabilitiesModel", new List<EmblemInteraction>());
        dangerThreshold = dataProvider.Get("dangerThreshold", 10);
    }

    public void Clear()
    {
    }
}
using Shop.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Shop.Controller
{
    public class ShopController
    {
        public ShopModel Model { get; private set; }

        public UserData UserData { get; private set; }

        private AnalyticsGameService _analytics;
        private GameConfigService _gameConfig;

        public ShopController(UserData userData, AnalyticsGameService analytics, GameConfigService gameConfig)
        {
            _gameConfig = gameConfig;
            _analytics = analytics;
            UserData = userData;
        }

        public void Initialize()
        {
            Load();
        }

        public void PurchaseItem(ShopItemModel model)
        {
            if (UserData.GetResourceAmount(model.Cost.Name) < model.Cost.Amount) return;

            UserData.RemoveResource(model.Cost);
            UserData.AddResource(model.Reward);
            UserData.Save();

            _analytics.SendEvent("purchasedItem", new Dictionary<string, object> { ["itemId"] = model.Id });
        }

        public void PurchaseHero(ShopItemModel model)
        {
            if (UserData.GetResourceAmount(model.Cost.Name) < model.Cost.Amount) return;

            UserData.RemoveResource(model.Cost);
            UserData.AddHero(model.Reward);
            UserData.Save();

            _analytics.SendEvent("purchasedItem", new Dictionary<string, object> { ["itemId"] = model.Id });
        }

        public void PurchaseBattleItem(ShopItemModel model)
        {
            if (UserData.GetResourceAmount(model.Cost.Name) < model.Cost.Amount) return;
            UserData.RemoveResource(model.Cost);
            UserData.AddBattleItem(model.Reward);
            UserData.Save();

            _analytics.SendEvent("purchasedItem", new Dictionary<string, object> { ["itemId"] = model.Id });
        }

        private void Load()
        {
            //Model = JsonUtility.FromJson<ShopModel>(Resources.Load<TextAsset>("ShopModel").text);
            Model = new ShopModel();
            Model.Items = _gameConfig.ShopOffers;
        }
    }
}

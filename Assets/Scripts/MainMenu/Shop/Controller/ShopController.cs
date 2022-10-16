using Shop.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Shop.Controller
{
    public class ShopController
    {
        public ShopModel Model { get; private set; }

        public UserData _userData { get; private set; }

        private AnalyticsGameService _analytics;
        private GameConfigService _gameConfig;
        private IIAPGameService _iapService;

        public ShopController(UserData userData, AnalyticsGameService analytics, GameConfigService gameConfig, IIAPGameService iapService)
        {
            _iapService = iapService;
            _gameConfig = gameConfig;
            _analytics = analytics;
            _userData = userData;
        }

        public void Initialize()
        {
            Load();
        }

        public async void PurchaseIAPGems(ShopItemModel item)
        {
            if (await _iapService.StartPurchase(item.IAPId))
            {
                _userData.AddResource(item.Reward);
                _userData.Save();
            }
            else
            {
                Debug.LogError("Purchase failed");
            }
        }

        public void PurchaseItem(ShopItemModel model)
        {
            if (_userData.GetResourceAmount(model.Cost.Name) < model.Cost.Amount) return;

            _userData.RemoveResource(model.Cost);
            _userData.AddResource(model.Reward);
            _userData.Save();

            _analytics.SendEvent("purchasedItem", new Dictionary<string, object> { ["itemId"] = model.Id });
        }

        public void PurchaseHero(ShopItemModel model)
        {
            if (_userData.GetResourceAmount(model.Cost.Name) < model.Cost.Amount) return;

            _userData.RemoveResource(model.Cost);
            _userData.AddHero(model.Reward);
            _userData.Save();

            _analytics.SendEvent("purchasedItem", new Dictionary<string, object> { ["itemId"] = model.Id });
        }

        public void PurchaseBattleItem(ShopItemModel model)
        {
            if (_userData.GetResourceAmount(model.Cost.Name) < model.Cost.Amount) return;
            _userData.RemoveResource(model.Cost);
            _userData.AddBattleItem(model.Reward);
            _userData.Save();

            _analytics.SendEvent("purchasedItem", new Dictionary<string, object> { ["itemId"] = model.Id });
        }

        private void Load()
        {
            Model = new ShopModel();
            Model.Items = _gameConfig.ShopOffers;
            Model.IAPs = _gameConfig.IAPOffers;
        }
    }
}

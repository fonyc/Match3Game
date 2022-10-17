using Shop.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Shop.Controller
{
    public class ShopController
    {
        public ShopModel Model { get; private set; }

        public GameProgressionService _gameProgressionService { get; private set; }

        private AnalyticsGameService _analytics;
        private GameConfigService _gameConfig;
        private IIAPGameService _iapService;

        public ShopController(GameProgressionService gameProgressionService, AnalyticsGameService analytics, GameConfigService gameConfig, IIAPGameService iapService)
        {
            _iapService = iapService;
            _gameConfig = gameConfig;
            _analytics = analytics;
            _gameProgressionService = gameProgressionService;
        }

        public void Initialize()
        {
            Load();
        }

        public async void PurchaseIAPGems(ShopItemModel item)
        {
            if (await _iapService.StartPurchase(item.IAPId))
            {
                _gameProgressionService.AddResource(item.Reward);
            }
            else
            {
                Debug.LogError("Purchase failed");
            }
        }

        public void PurchaseItem(ShopItemModel model)
        {
            if (_gameProgressionService.GetResourceAmount(model.Cost.Name) < model.Cost.Amount) return;

            _gameProgressionService.RemoveResource(model.Cost);
            _gameProgressionService.AddResource(model.Reward);

            _analytics.SendEvent("purchasedItem", new Dictionary<string, object> { ["itemId"] = model.Id });
        }

        public void PurchaseHero(ShopItemModel model)
        {
            if (_gameProgressionService.GetResourceAmount(model.Cost.Name) < model.Cost.Amount) return;

            _gameProgressionService.RemoveResource(model.Cost);
            _gameProgressionService.AddHero(model.Reward);

            _analytics.SendEvent("purchasedItem", new Dictionary<string, object> { ["itemId"] = model.Id });
        }

        public void PurchaseBattleItem(ShopItemModel model)
        {
            if (_gameProgressionService.GetResourceAmount(model.Cost.Name) < model.Cost.Amount) return;
            _gameProgressionService.RemoveResource(model.Cost);
            _gameProgressionService.AddBattleItem(model.Reward);

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

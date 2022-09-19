using Shop.Controller;
using Shop.Model;
using UnityEngine;

namespace Shop.View
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField]
        private ShopItemView _shopItemPrefab = null;

        [SerializeField]
        private Transform _itemsParent = null;

        private ShopController _controller;

        public void Initialize(ShopController controller, UserData userData)
        {
            _controller = controller;

            while (_itemsParent.childCount > 0)
            {
                Transform child = _itemsParent.GetChild(0);
                child.SetParent(null);
                Destroy(child.gameObject);
            }

            foreach (ShopItemModel shopItemModel in _controller.Model.Items)
            {
                Instantiate(_shopItemPrefab, _itemsParent).SetData(shopItemModel, userData, OnPurchaseItem);
            }
        }

        private void OnPurchaseItem(ShopItemModel model)
        {
            switch (model.Reward.Type)
            {
                case "Hero":
                _controller.PurchaseHero(model);
                    break;
                case "BattleItem":
                    _controller.PurchaseBattleItem(model);
                    break;
                default:
                _controller.PurchaseItem(model);
                    break;
            }
        }
    }
}

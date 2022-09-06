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

            //Ensure there are no previous items
            while (_itemsParent.childCount > 0)
            {
                Transform child = _itemsParent.GetChild(0);
                child.SetParent(null);
                Destroy(child.gameObject);
            }

            //Instantiate the items in the store
            foreach (ShopItemModel shopItemModel in _controller.Model.Items)
            {
                Instantiate(_shopItemPrefab, _itemsParent).SetData(shopItemModel, userData, OnPurchaseItem);
            }
        }

        public void AddGold()
        {
            _controller.FreeResource();
        }

        private void OnPurchaseItem(ShopItemModel model)
        {
            _controller.PurchaseItem(model);
        }
    }
}

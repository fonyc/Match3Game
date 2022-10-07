using DG.Tweening;
using Shop.Controller;
using Shop.Model;
using System.Collections;
using UnityEngine;

namespace Shop.View
{
    public class ShopView : MonoBehaviour, IMainMenuAnimation
    {
        [SerializeField]
        private ShopItemView _shopItemPrefab = null;

        [SerializeField]
        private Transform _itemsParent = null;

        private ShopController _controller;

        public string Id { get => "Shop"; set { } }

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

        public void AppearAnimation(RectTransform rect, float delay)
        {
            gameObject.SetActive(true);
            StartCoroutine(AppearAnimation_Coro(rect, delay));
        }

        public IEnumerator AppearAnimation_Coro(RectTransform rect, float delay)
        {
            yield return new WaitForSeconds(delay);
            rect.DOAnchorPos(Vector2.zero, 0.3f).SetEase(Ease.OutBack);
        }

        public void HideAnimation(RectTransform rect)
        {
            rect.DOAnchorPos(new Vector2(2500f, 0), 0.25f).SetEase(Ease.InBack).OnComplete(Hide);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
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

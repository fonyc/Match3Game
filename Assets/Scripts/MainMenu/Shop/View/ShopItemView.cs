using Shop.Model;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Shop.View
{
    public class ShopItemView : MonoBehaviour
    {
        #region UI FIELDS
        [SerializeField]
        private Image _image = null;

        [SerializeField]
        private TMP_Text _title = null;

        [SerializeField]
        private TMP_Text _amount = null;

        [SerializeField]
        private Image _costImage = null;

        [SerializeField]
        private TMP_Text _costText = null;

        [SerializeField]
        private Button _button = null;
        #endregion

        private ShopItemModel _model;
        private GameProgressionService _gameProgression;
        private Action<ShopItemModel> _onClickedEvent;

        public void SetData(ShopItemModel model, GameProgressionService gameProgression, Action<ShopItemModel> onClickedEvent)
        {
            _model = model;
            _onClickedEvent = onClickedEvent;
            _gameProgression = gameProgression;
            _gameProgression.OnResourceModified += InventoryUpdated;
            UpdateVisuals();
        }

        private void OnDestroy()
        {
            if (_gameProgression != null) _gameProgression.OnResourceModified -= InventoryUpdated;
        }

        private void InventoryUpdated(string resource)
        {
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (_model == null) return;

            Addressables.LoadAssetAsync<Sprite>(_model.Image).Completed += handler =>
            {
                _image.sprite = handler.Result;
            };
            
            Addressables.LoadAssetAsync<Sprite>(_model.Cost.Name).Completed += handler =>
            {
                _costImage.sprite = handler.Result;
            };

            _title.text = _model.Title;
            _amount.text = "x " + _model.Reward.Amount.ToString();
            _costText.text = _model.Cost.Amount.ToString();
            _costText.color = CanPay() ? Color.white : Color.red;
            _button.interactable = CanPay() ? true : false;
            _button.interactable = !IsHeroOwned();
        }

        private bool CanPay() => _gameProgression.GetResourceAmount(_model.Cost.Name) >= _model.Cost.Amount;

        private bool IsHeroOwned() 
        {
            foreach(OwnedHero hero in _gameProgression.GetOwnedHeroes())
            {
                if (hero.Id == _model.Reward.Name) return true;
            }
            return false;
        }

        public void OnClicked()
        {
            if (_model == null) return;

            if (!CanPay()) return;

            _onClickedEvent?.Invoke(_model);
        }
    }
}

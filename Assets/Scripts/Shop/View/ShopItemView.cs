using Shop.Model;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.View
{
    public class ShopItemView : MonoBehaviour
    {
        #region UI FIELDS
        [SerializeField]
        private List<Sprite> _imageSprites = new List<Sprite>();

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
        #endregion

        private ShopItemModel _model;
        private UserData _userData;
        private Action<ShopItemModel> _onClickedEvent;

        public void SetData(ShopItemModel model, UserData userData, Action<ShopItemModel> onClickdEvent)
        {
            _model = model;
            _onClickedEvent = onClickdEvent;
            _userData = userData;
            _userData.OnResourceModified += InventoryUpdated;
            UpdateVisuals();
        }

        private void OnDestroy()
        {
            if (_userData != null) _userData.OnResourceModified -= InventoryUpdated;
        }

        private void InventoryUpdated(string resource)
        {
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (_model == null) return;

            _image.sprite = _imageSprites.Find(sprite => sprite.name == _model.Image);
            _costImage.sprite = _imageSprites.Find(sprite => sprite.name == _model.Cost.Type);
            _title.text = _model.Title;
            _amount.text = "x " + _model.Reward.Amount.ToString();
            _costText.text = _model.Cost.Amount.ToString();
            _costText.color = CanPay() ? Color.white : Color.red;
        }

        private bool CanPay() => _userData.GetResourceAmount(_model.Cost.Type) >= _model.Cost.Amount;

        public void OnClicked()
        {
            if (_model == null) return;

            if (!CanPay()) return;

            _onClickedEvent?.Invoke(_model);

        }
    }

}

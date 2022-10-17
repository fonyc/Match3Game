using Shop.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopIAPView : MonoBehaviour
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
    private TMP_Text _costText = null;

    [SerializeField]
    private Button _button;
    #endregion

    private ShopItemModel _model;
    private GameProgressionService _gameProgression;
    private IIAPGameService _iapService;
    private Action<ShopItemModel> _onClickedEvent;

    public void SetData(ShopItemModel model, GameProgressionService gameProgression, IIAPGameService iapService, 
        Action<ShopItemModel> OnPurchaseItem)
    {
        _onClickedEvent = OnPurchaseItem;
        _iapService = iapService;
        _model = model;
        _gameProgression = gameProgression;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (_model == null) return;

        _title.text = _model.Title;
        _image.sprite = _imageSprites.Find(sprite => sprite.name == _model.Image);
        _amount.text = "x " + _model.Reward.Amount.ToString();
        _costText.text = _model.Cost.Amount.ToString();

        StopAllCoroutines();
        StartCoroutine(WaitForIAPReady());
    }

    public void OnClicked()
    {
        _onClickedEvent?.Invoke(_model);
    }

    IEnumerator WaitForIAPReady()
    {
        _costText.text = "Loading...";
        _button.interactable = false;

        while (!_iapService.IsReady())
        {
            yield return new WaitForSeconds(0.5f);
        }

        _button.interactable = true;
        _costText.text = _iapService.GetLocalizedPrice(_model.IAPId);
    }
}
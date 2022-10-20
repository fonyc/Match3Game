using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ItemViewSlot : MonoBehaviour
{
    [Header(" --- UI ---")]
    [Space(5)]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemQty = null;

    BattleItemModel _model;
    int _amount;
    private event Action<BattleItemModel> _onClickedEvent;

    public void SetData(BattleItemModel model, int ownedQty, int maxQty, Action<BattleItemModel> onClickedEvent)
    {
        _model = model;
        _amount =  ownedQty >= maxQty? maxQty : ownedQty;
        _onClickedEvent = onClickedEvent;

        UpdateItemSprite();
        UpdateItemQty();
    }

    public void UpdateItemSprite()
    {
        Addressables.LoadAssetAsync<Sprite>(_model.AvatarImage).Completed += handler =>
        {
            _itemIcon.sprite = handler.Result;
        };
    }

    public void UpdateItemQty()
    {
        _itemQty.text = _amount.ToString();
    }

    private bool CanUse() => _amount > 0;

    public void OnClicked()
    {
        if (_model == null) return;

        if (!CanUse()) return;

        _amount = _amount - 1 == 0 ? 0 : _amount - 1;
        UpdateItemQty();

        _onClickedEvent?.Invoke(_model);
    }
}

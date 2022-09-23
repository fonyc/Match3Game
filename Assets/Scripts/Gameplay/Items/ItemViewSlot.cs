using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemViewSlot : MonoBehaviour
{
    [Header(" --- UI ---")]
    [Space(5)]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemQty = null;

    [SerializeField] private List<Sprite> ItemSpriteList = new();
    BattleItemModel _model;
    int _amount;
    private event Action<BattleItemModel> _onClickedEvent;
    
    public void SetData(BattleItemModel model, int ownedQty, int maxQty, Action<BattleItemModel> onClickedEvent)
    {
        _model = model;
        _amount =  ownedQty >= maxQty? maxQty : ownedQty;
        _onClickedEvent = onClickedEvent;
        UpdateItemSprite();
        UpdateItemQty(_amount);
    }

    public void UpdateItemSprite()
    {
        _itemIcon.sprite = ItemSpriteList.Find(sprite => sprite.name == _model.AvatarImage);
    }

    public void UpdateItemQty(int qty)
    {
        _itemQty.text = _amount.ToString();
    }

    private bool CanUse() => _amount > 0;

    public void OnClicked()
    {
        if (_model == null) return;

        if (!CanUse()) return;

        _onClickedEvent?.Invoke(_model);
    }
}

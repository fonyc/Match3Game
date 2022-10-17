using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamBattleItemView : MonoBehaviour
{
    [SerializeField]
    List<Sprite> spriteItems = new();

    [SerializeField]
    TMP_Text _itemName = null;

    [SerializeField]
    TMP_Text _amount = null;

    [SerializeField]
    private Image _itemImage = null;

    [SerializeField]
    GameObject selectedTick = null;

    BattleItemModel _battleItemModel;

    private event Action<string> _onClickedEvent;
    private GameProgressionService _gameProgression;
    private int qty => _gameProgression.GetBattleItemAmount(_battleItemModel.Id);

    public void SetData(BattleItemModel itemModel, GameProgressionService gameProgression, Action<string> onClickedEvent)
    {
        _gameProgression = gameProgression;
        _battleItemModel = itemModel;
        _onClickedEvent = onClickedEvent;
        _itemImage.sprite = spriteItems.Find(sprite => sprite.name == _battleItemModel.AvatarImage);
        _itemName.text = itemModel.Name;
        _amount.text = "x " + qty.ToString();
    }

    public string GetId()
    {
        return _battleItemModel.Id;
    }

    public void OnClicked()
    {
        _onClickedEvent?.Invoke(_battleItemModel.Id);
    }

    public void SetTick(bool value)
    {
        selectedTick.SetActive(value);
    }
}

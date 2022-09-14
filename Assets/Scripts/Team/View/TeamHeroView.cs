using Shop.Model;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamHeroView : MonoBehaviour
{
    [SerializeField]
    List<Sprite> heroList = new();

    [SerializeField]
    TMP_Text _heroName = null;

    [SerializeField]
    private Image _heroAvatar = null;

    [SerializeField]
    GameObject selectedTick = null;

    HeroItemModel _heroModel;

    private event Action<string> _onClickedEvent;


    public void SetData(HeroItemModel heroModel, Action<string> onClickedEvent)
    {
        _heroModel = heroModel;
        _onClickedEvent = onClickedEvent;
        _heroAvatar.sprite = heroList.Find(sprite => sprite.name == _heroModel.AvatarImage);
        _heroName.text = heroModel.Name;
    }

    public string GetId()
    {
        return _heroModel.Id;
    }

    public void OnClicked()
    {
        _onClickedEvent?.Invoke(_heroModel.Id);
    }

    public void SetTick(bool value)
    {
        selectedTick.SetActive(value);
    }
}
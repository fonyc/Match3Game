using Shop.Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamHeroView : MonoBehaviour
{
    List<Image> heroList = new();

    public void SetData(HeroItemModel model, UserData userData, Action<ShopItemModel> onClickedEvent)
    {
        //_model = model;
        //_onClickedEvent = onClickedEvent;
        //_userData = userData;
        //_userData.OnResourceModified += InventoryUpdated;
        //UpdateVisuals();
    }

    public void OnClicked()
    {
        //_onClickedEvent?.Invoke(_model);
    }
}

using Shop.Controller;
using Shop.Model;
using Shop.View;
using UnityEngine;

public class HeroesView : MonoBehaviour
{
    [SerializeField]
    private HeroItemView _heroItemPrefab = null;

    [SerializeField]
    private Transform _itemsParent = null;

    private HeroesController _controller;

    public void Initialize(HeroesController controller, UserData userData)
    {
        _controller = controller;

        //Ensure there are no previous items
        while (_itemsParent.childCount > 0)
        {
            Transform child = _itemsParent.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        //Instantiate the owned heroes 
        foreach (OwnedHero ownedHero in userData.GetOwnedHeroList())
        {
            Instantiate(_heroItemPrefab, _itemsParent);//.SetData(shopItemModel, userData, OnPurchaseItem);
        }
    }

    //private void OnPurchaseItem(ShopItemModel model)
    //{
    //    _controller.PurchaseItem(model);
    //}
}

using Shop.View;
using UnityEngine;

public class BottomBarController : MonoBehaviour
{
    GameObject currentOpenedTab = null;

    [SerializeField] ShopView shopTab;
    [SerializeField] HeroesView heroesTab;

    public void AddTab(object tab)
    {
        switch (tab)
        {
            case ShopView shop:
                shopTab = (ShopView)tab;
                break;
            case HeroesView heroes:
                heroesTab = (HeroesView)tab;
                break;
        }
    }

    private GameObject GetTabByName(string tabName)
    {
        switch (tabName)
        {
            case "Shop":
                return shopTab.gameObject;
            case "Heroes":
                return  heroesTab.gameObject; 
            case "Levels":
                return null;
            case "Equip":
                return null;
            default:
                return null;
        }
    }

    public void OpenTab(string tabName)
    {
        GameObject newTab = GetTabByName(tabName);
        if (newTab == null) return;

        if (currentOpenedTab == null)
        {
            newTab.SetActive(true);
            currentOpenedTab = newTab;
        }
        else if (newTab == currentOpenedTab)
        {
            newTab.SetActive(false);
            currentOpenedTab = null;
        }
        else
        {
            newTab.SetActive(true);
            currentOpenedTab.SetActive(false);
            currentOpenedTab = newTab;
        }
    }
}

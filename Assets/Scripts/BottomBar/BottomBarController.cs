using Shop.View;
using UnityEngine;

public class BottomBarController : MonoBehaviour
{
    GameObject currentOpenedTab = null;

    [SerializeField] ShopView shopTab;
    
    public void AddTab(object tab)
    {
        switch (tab)
        {
            case ShopView shop:
                shopTab = (ShopView)tab;
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
                return null;
            case "Missions":
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
            currentOpenedTab = newTab;
        }
    }
}

using Shop.View;
using UnityEngine;
using DG.Tweening;
using Board.View;
using System.Collections.Generic;

public class BottomBarController : MonoBehaviour
{
    GameObject currentOpenedTab = null;

    List<GameObject> tabs = new();

    public void AddTab(GameObject tab)
    {
        tabs.Add(tab);
    }

    private GameObject GetTabById(string tabName)
    {
        return tabs.Find(tab => tab.GetComponent<IMainMenuAnimation>().Id == tabName);
    }

    public void OpenTab(string tabName)
    {
        GameObject newTab = GetTabById(tabName);
        if (newTab == null) return;

        if (currentOpenedTab == null)
        {
            newTab.GetComponent<IMainMenuAnimation>().AppearAnimation(newTab.GetComponent<RectTransform>(), 0f);
            currentOpenedTab = newTab;
        }
        else if (newTab == currentOpenedTab)
        {
            currentOpenedTab = null;
            newTab.GetComponent<IMainMenuAnimation>().HideAnimation(newTab.GetComponent<RectTransform>());
        }
        else
        {
            currentOpenedTab.GetComponent<IMainMenuAnimation>().HideAnimation(currentOpenedTab.GetComponent<RectTransform>());
            newTab.GetComponent<IMainMenuAnimation>().AppearAnimation(newTab.GetComponent<RectTransform>(), 0.25f);
            currentOpenedTab = newTab;
        }
    }
}

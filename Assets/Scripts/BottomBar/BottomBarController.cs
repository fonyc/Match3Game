using Shop.View;
using UnityEngine;

public class BottomBarController : MonoBehaviour
{
    GameObject currentOpenedTab = null;

    [SerializeField] ShopView shopTab;
    [SerializeField] HeroesView heroesTab;
    [SerializeField] TeamView teamTab;
    //[SerializeField] LevelView levelTab;

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
            case TeamView team:
                teamTab = (TeamView)tab;
                break;
            //case LevelView level:
            //    levelsTab = (LevelView)tab;
            //    break;
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
            case "Team":
                return teamTab.gameObject;
            case "Levels":
                return null;
                //return levelsTab.gameObject;
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

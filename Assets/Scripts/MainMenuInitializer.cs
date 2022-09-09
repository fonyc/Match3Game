using Shop.Controller;
using Shop.View;
using UnityEngine;

public class MainMenuInitializer : MonoBehaviour
{
    #region SERIALIZED FIELDS
    [Header("--- PREFABS ---")]
    [SerializeField]
    private ResourcesView _topBarResourcesPrefab = null;

    [SerializeField]
    private ShopView _shopViewPrefab = null;

    [SerializeField]
    private HeroesView _heroesViewPrefab = null;

    [SerializeField]
    private BottomBarController _bottomBarPrefab = null;
    #endregion

    #region INJECTIONS
    private UserData _userData = null;
    private ShopController _shopController = null;
    private HeroesController _heroesController = null;
    #endregion

    private void Start()
    {
        _userData = new UserData();
        _shopController = new ShopController(_userData);
        _heroesController = new HeroesController(_userData);

        //Create bottom main menu
        BottomBarController bottomBar = Instantiate(_bottomBarPrefab, transform);

        _userData.Load();

        //Initialize resources top bar
        Instantiate(_topBarResourcesPrefab, transform).Initialize(_userData);

        //Initialize controllers
        _shopController.Initialize();
        _heroesController.Initialize();

        //Initialize tabs and disable them for later

        //SHOP TAB
        ShopView shop = Instantiate(_shopViewPrefab, transform); 
        shop.Initialize(_shopController, _userData);
        bottomBar.AddTab(shop);
        shop.gameObject.SetActive(false);

        //HEROE COLLECTION TAB
        HeroesView heroesView = Instantiate(_heroesViewPrefab, transform);
        heroesView.Initialize(_heroesController, _userData);
        bottomBar.AddTab(heroesView);
        heroesView.gameObject.SetActive(false);
    }
}

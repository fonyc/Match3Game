using Shop.Controller;
using Shop.View;
using UnityEngine;

public class MainMenuInitializer : MonoBehaviour
{
    [SerializeField]
    private ResourcesView _topBarResourcesPrefab = null;
    [SerializeField]
    private ShopView _shopViewPrefab = null;

    private UserData _userData = null;
    private ShopController _shopController = null;

    [SerializeField]
    private BottomBarController _bottomBarPrefab = null;

    private void Start()
    {
        //create systems
        _userData = new UserData();
        _shopController = new ShopController(_userData);

        //Create bottom main menu
        BottomBarController bottomBar = Instantiate(_bottomBarPrefab, transform);

        //Initialize data
        _userData.Load();

        //Initialize resources top bar
        Instantiate(_topBarResourcesPrefab, transform).Initialize(_userData);

        //Initialize controllers
        _shopController.Initialize();

        //Initialize tabs and disable them for later
        ShopView shop = Instantiate(_shopViewPrefab, transform); 
        shop.Initialize(_shopController, _userData);
        bottomBar.AddTab(shop);
        shop.gameObject.SetActive(false);
    }
}

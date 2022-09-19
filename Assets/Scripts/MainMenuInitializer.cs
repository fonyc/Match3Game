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
    private TeamView _teamViewPrefab = null;

    [SerializeField]
    private LevelsView _levelsViewPrefab = null;

    [SerializeField]
    private BottomBarController _bottomBarPrefab = null;

    [SerializeField]
    private SceneLoader _sceneLoaderPrefab = null;
    #endregion

    #region INJECTIONS
    private UserData _userData = null;
    private ShopController _shopController = null;
    private HeroesController _heroesController = null;
    private TeamController _teamController = null;
    private LevelsController _levelController = null;
    #endregion

    private void Start()
    {
        SceneLoader sceneLoader = Instantiate(_sceneLoaderPrefab);

        _userData = new UserData();
        _shopController = new ShopController(_userData);
        _heroesController = new HeroesController(_userData);
        _teamController = new TeamController(_userData);
        _levelController = new LevelsController(_userData, sceneLoader);

        //Create bottom main menu
        BottomBarController bottomBar = Instantiate(_bottomBarPrefab, transform);

        _userData.Load();

        //Initialize resources top bar
        Instantiate(_topBarResourcesPrefab, transform).Initialize(_userData);

        #region INIT CONTROLLERS
        _shopController.Initialize();
        _heroesController.Initialize();
        _teamController.Initialize();
        _levelController.Initialize();
        #endregion

        #region INIT TABS

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

        //TEAM TAB
        TeamView teamView = Instantiate(_teamViewPrefab, transform);
        teamView.Initialize(_teamController,_userData);
        bottomBar.AddTab(teamView);
        teamView.gameObject.SetActive(false);

        //LEVELS TAB
        LevelsView levelsView = Instantiate(_levelsViewPrefab, transform);
        levelsView.Initialize(_levelController, _userData);
        bottomBar.AddTab(levelsView);
        levelsView.gameObject.SetActive(false);

        #endregion
    }
}

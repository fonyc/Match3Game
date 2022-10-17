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

    [SerializeField] 
    private TermsAndConditions _termsPrefab = null; 
    #endregion

    //SERVICES
    private AnalyticsGameService _analytics = null;
    private GameConfigService _gameConfigService = null;
    private IIAPGameService _iapService = null;

    #region INJECTIONS
    private GameProgressionService _gameProgressionService = null;
    private ShopController _shopController = null;
    private HeroesController _heroesController = null;
    private TeamController _teamController = null;
    private LevelsController _levelController = null;
    #endregion

    private void Awake()
    {

        _analytics = ServiceLocator.GetService<AnalyticsGameService>();
        _gameConfigService = ServiceLocator.GetService<GameConfigService>();
        _iapService = ServiceLocator.GetService<IIAPGameService>();
        _gameProgressionService = ServiceLocator.GetService<GameProgressionService>();

        SceneLoader sceneLoader = Instantiate(_sceneLoaderPrefab);
        _shopController = new ShopController(_gameProgressionService, _analytics, _gameConfigService, _iapService);
        _heroesController = new HeroesController(_gameConfigService);
        _teamController = new TeamController(_gameProgressionService, _gameConfigService);
        _levelController = new LevelsController(_gameProgressionService, sceneLoader, _gameConfigService);
    }

    private void Start()
    {
        //Terms and Conditions
        TermsAndConditions terms = Instantiate(_termsPrefab, transform);
        terms.Initialize(_gameConfigService);

        //Create bottom main menu
        BottomBarController bottomBar = Instantiate(_bottomBarPrefab, transform);

        //_gameProgressionService.Load();

        //Initialize resources top bar
        ResourcesView resourcesView = Instantiate(_topBarResourcesPrefab, transform);
        resourcesView.Initialize(_gameProgressionService);

        #region INIT CONTROLLERS
        _shopController.Initialize();
        _heroesController.Initialize();
        _teamController.Initialize();
        _levelController.Initialize();
        #endregion

        #region INIT TABS

        //HEROE COLLECTION TAB
        HeroesView heroesView = Instantiate(_heroesViewPrefab, transform);
        heroesView.Initialize(_heroesController, _gameProgressionService);
        bottomBar.AddTab(heroesView.gameObject);

        //SHOP TAB
        ShopView shop = Instantiate(_shopViewPrefab, transform); 
        shop.Initialize(_shopController, _gameProgressionService, _iapService);
        bottomBar.AddTab(shop.gameObject);

        //TEAM TAB
        TeamView teamView = Instantiate(_teamViewPrefab, transform);
        teamView.Initialize(_teamController,_gameProgressionService);
        bottomBar.AddTab(teamView.gameObject);

        //LEVELS TAB
        LevelsView levelsView = Instantiate(_levelsViewPrefab, transform);
        levelsView.Initialize(_levelController, _gameProgressionService);
        bottomBar.AddTab(levelsView.gameObject);

        bottomBar.transform.SetAsLastSibling();
        resourcesView.transform.SetAsLastSibling();
        terms.transform.SetAsLastSibling();
        #endregion
    }
}

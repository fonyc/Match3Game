using Board.Controller;
using Board.View;
using System.Collections.Generic;
using UnityEngine;

public class GameplayInitializer : MonoBehaviour
{
    [Header("--- BOARD SIZE ---")]
    [Space(5)]
    [SerializeField] private Vector2Int _boardSize = new Vector2Int(8, 8);

    [Header("--- PREFABS ---")]
    [Space(5)]
    [SerializeField] private BoardView _boardViewPrefab = null;
    [SerializeField] private PlayerView _playerViewPrefab = null;
    [SerializeField] private ItemView _itemViewPrefab = null;
    [SerializeField] private SkillView _skillViewPrefab = null;
    [SerializeField] private SceneLoader _sceneLoaderPrefab = null;
    [SerializeField] private EnemyView _enemyViewPrefab = null;
    [SerializeField] private MenuPauseView _menuPausePrefab = null;
    [SerializeField] private RoundView _roundViewPrefab = null;
    [SerializeField] private MatchView _matchViewPrefab = null;

    [Header("--- INPUTS ---")]
    [Space(5)]
    [SerializeField] List<BoardInput> inputs = new();

    [Header("--- SKILLS ---")]
    [Space(5)]
    [SerializeField] private List<Skill> SkillList = new();

    [Header("--- SKILLS ---")]
    [Space(5)]
    [SerializeField] private MatchReport _matchReport;

    private SkillController _skillController;
    private BoardController _boardController;
    private PlayerController _playerController;
    private ItemController _itemController;
    private CombatController _combatController;
    private EnemyController _enemyController;
    private MatchController _matchController;
    private SceneLoader _sceneLoader;

    [Header("--- EVENT BUS ---")]
    [Space(5)]
    [SerializeField] private StatTripleIntArgument_Event _onPlayerAttacks;
    [SerializeField] private StatIntIntArgument_Event _onEnemyAttacks;
    [SerializeField] private TripleIntArgument_Event _onEmblemsDestroyed;
    [SerializeField] private IntArgument_Event _onMovesAvailableChanged;
    [SerializeField] private NoArgument_Event _onPlayerDied;
    [SerializeField] private NoArgument_Event _onPlayerWin;
    [SerializeField] private NoArgument_Event _onPlayerRecievedDamage;
    [SerializeField] private NoArgument_Event _onBuffsReset;

    private GameConfigService _gameConfigService;
    private AdsGameService _adsService;
    private AnalyticsGameService _analytics;
    private UserData _userData;

    private void Awake()
    {
        //Init user data
        _analytics = ServiceLocator.GetService<AnalyticsGameService>();
        _gameConfigService = ServiceLocator.GetService<GameConfigService>();
        _adsService = ServiceLocator.GetService<AdsGameService>();
        _userData = new UserData();
        _userData.Load();
        _sceneLoader = Instantiate(_sceneLoaderPrefab);

        //Controllers creation
        _combatController = new CombatController(_gameConfigService);

        _enemyController = new EnemyController(_userData, _combatController, _onEnemyAttacks, 
            _onPlayerWin, _gameConfigService, _matchReport);

        _skillController = new SkillController(_userData, SkillList, _gameConfigService);

        _boardController = new BoardController(_boardSize.x, _boardSize.y, _skillController, inputs, 
            _userData, _onEmblemsDestroyed, _onMovesAvailableChanged, _gameConfigService, _matchReport);

        _itemController = new ItemController(_userData, _gameConfigService);

        _playerController = new PlayerController(_userData, _itemController, _gameConfigService,
            _combatController, _onPlayerAttacks, _onPlayerDied, _onPlayerRecievedDamage, _matchReport);

        _matchController = new MatchController(_gameConfigService, _userData, _sceneLoader, _adsService, _analytics);
    }

    void Start()
    {
        //Instantiate dependencies with no controller
        Instantiate(_roundViewPrefab, transform).Initialize(_onMovesAvailableChanged, _gameConfigService);

        //Init. controllers
        _combatController.Initialize();
        _skillController.Initialize();
        _enemyController.Initialize();
        _playerController.Initialize();
        _itemController.Initialize();
        _boardController.Initialize();
        _matchController.Initialize();

        Instantiate(_skillViewPrefab, transform).Initialize(_skillController, _onPlayerAttacks);
        Instantiate(_boardViewPrefab).Initialize(_boardController, _boardSize);

        Instantiate(_playerViewPrefab, transform).Initialize(_playerController, _onEmblemsDestroyed, 
            _onEnemyAttacks, _onBuffsReset);

        Instantiate(_itemViewPrefab, transform).Initialize(_itemController);

        Instantiate(_enemyViewPrefab, transform).Initialize(_enemyController, _onPlayerAttacks, 
            _onMovesAvailableChanged);

        Instantiate(_menuPausePrefab, transform);

        MatchView matchView = Instantiate(_matchViewPrefab, transform);
        matchView.Initialize(_matchController, _onPlayerDied, _onPlayerWin, _onPlayerRecievedDamage, _matchReport, _onBuffsReset);
        matchView.transform.SetAsLastSibling();
    }
}

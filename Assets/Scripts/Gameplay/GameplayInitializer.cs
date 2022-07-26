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

    [Header("--- MATCH REPORT ---")]
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
    [SerializeField] private StringArgument_Event _onInputChange;

    private GameConfigService _gameConfigService;
    private AdsGameService _adsService;
    private AnalyticsGameService _analytics;
    private GameProgressionService _gameProgression;

    private void Awake()
    {
        _analytics = ServiceLocator.GetService<AnalyticsGameService>();
        _gameConfigService = ServiceLocator.GetService<GameConfigService>();
        _adsService = ServiceLocator.GetService<AdsGameService>();
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();

        _sceneLoader = Instantiate(_sceneLoaderPrefab);

        _combatController = new CombatController(_gameConfigService);
        _enemyController = new EnemyController(_gameProgression, _combatController, _onEnemyAttacks, 
            _onPlayerWin, _gameConfigService, _matchReport);
        _itemController = new ItemController(_gameProgression, _gameConfigService);
        _skillController = new SkillController(_gameProgression, SkillList, _gameConfigService, _itemController);
        _boardController = new BoardController(_boardSize.x, _boardSize.y, _skillController, inputs, 
            _gameProgression, _onEmblemsDestroyed, _onMovesAvailableChanged, _gameConfigService, _matchReport, _onInputChange);
        _playerController = new PlayerController(_gameProgression, _itemController, _gameConfigService,
            _combatController, _onPlayerAttacks, _onPlayerDied, _onPlayerRecievedDamage, _matchReport);
        _matchController = new MatchController(_gameConfigService, _gameProgression, _sceneLoader, _adsService, _analytics);
    }

    void Start()
    {
        Instantiate(_roundViewPrefab, transform).Initialize(_onMovesAvailableChanged, _gameConfigService);

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
        matchView.Initialize(_matchController, _onPlayerDied, _onPlayerWin, _onPlayerRecievedDamage, 
            _matchReport, _onBuffsReset, _onInputChange);
        matchView.transform.SetAsLastSibling();
    }
}

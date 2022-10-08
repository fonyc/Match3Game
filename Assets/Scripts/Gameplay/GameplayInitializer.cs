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
    [SerializeField] private StatIntIntArgument_Event _onPlayerAttacks;
    [SerializeField] private StatIntIntArgument_Event _onEnemyAttacks;
    [SerializeField] private DoubleIntArgument_Event _onEmblemsDestroyed;
    [SerializeField] private IntArgument_Event _onMovesAvailableChanged;
    [SerializeField] private NoArgument_Event _onPlayerDied;
    [SerializeField] private NoArgument_Event _onPlayerWin;

    private GameConfigService _gameConfigService;
    private UserData _userData;

    private void Awake()
    {
        //Init user data
        _gameConfigService = ServiceLocator.GetService<GameConfigService>();
        _userData = new UserData();
        _userData.Load();
        _sceneLoader = Instantiate(_sceneLoaderPrefab);

        //Controllers creation
        _combatController = new CombatController();
        _enemyController = new EnemyController(_userData, _combatController, _onEnemyAttacks, _onPlayerWin);
        _skillController = new SkillController(_userData, SkillList);
        _boardController = new BoardController(_boardSize.x, _boardSize.y, _skillController, inputs, _userData, _onEmblemsDestroyed, _onMovesAvailableChanged);
        _itemController = new ItemController(_userData);
        _playerController = new PlayerController(_userData, _itemController, _combatController, _onPlayerAttacks, _onPlayerDied);
        _matchController = new MatchController(_gameConfigService, _userData, _sceneLoader);
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

        Instantiate(_skillViewPrefab, transform).Initialize(_skillController);
        Instantiate(_boardViewPrefab).Initialize(_boardController, _boardSize);
        Instantiate(_playerViewPrefab, transform).Initialize(_playerController, _onEmblemsDestroyed, _onEnemyAttacks);
        Instantiate(_itemViewPrefab, transform).Initialize(_itemController);
        Instantiate(_enemyViewPrefab, transform).Initialize(_enemyController, _onPlayerAttacks, _onMovesAvailableChanged);
        Instantiate(_menuPausePrefab, transform);
        MatchView matchView = Instantiate(_matchViewPrefab, transform);
        matchView.Initialize(_matchController, _onPlayerDied, _onPlayerWin);
        matchView.transform.SetAsLastSibling();
    }
}

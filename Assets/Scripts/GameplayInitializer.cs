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
    [SerializeField] private Gameplay_TopBar _topBar = null;

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

    [Header("--- EVENT BUS ---")]
    [Space(5)]
    [SerializeField] StatIntIntArgument_Event _onPlayerAttacks;
    [SerializeField] DoubleIntArgument_Event _onEmblemsDestroyed;


    private UserData _userData;

    private void Awake()
    {
        //Init user data
        _userData = new UserData();
        _userData.Load();

        //Controllers creation
        _combatController = new CombatController();
        _enemyController = new EnemyController(_userData, _combatController);
        _skillController = new SkillController(_userData, SkillList);
        _boardController = new BoardController(_boardSize.x, _boardSize.y, _skillController, inputs, _userData, _onEmblemsDestroyed);
        _itemController = new ItemController(_userData);
        _playerController = new PlayerController(_userData, _itemController, _combatController, _onPlayerAttacks);
    }

    void Start()
    {
        //Init. controllers
        _combatController.Initialize();
        _skillController.Initialize();
        _enemyController.Initialize();
        _playerController.Initialize();
        _itemController.Initialize();
        _boardController.Initialize();

        Instantiate(_skillViewPrefab, transform).Initialize(_skillController);
        Instantiate(_boardViewPrefab).Initialize(_boardController, _boardSize);
        Instantiate(_playerViewPrefab, transform).Initialize(_playerController, _onEmblemsDestroyed);
        Instantiate(_itemViewPrefab, transform).Initialize(_itemController);
        Instantiate(_enemyViewPrefab, transform).Initialize(_enemyController, _onPlayerAttacks);
        Instantiate(_topBar, transform);
        Instantiate(_sceneLoaderPrefab);
    }

}

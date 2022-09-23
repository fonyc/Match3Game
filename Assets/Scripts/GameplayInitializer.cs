using Board.Controller;
using Board.View;
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
    [SerializeField] private PlayerView _skillViewPrefab = null;
    [SerializeField] private SceneLoader _sceneLoaderPrefab = null;
    [SerializeField] private EnemyView _enemyViewPrefab = null;
    [SerializeField] private Gameplay_TopBar _topBar = null;


    private BoardController _boardController;
    private PlayerController _playerController;
    private ItemController _itemController;

    private UserData _userData;

    void Start()
    {
        //Init user data
        _userData = new UserData();
        _userData.Load();

        //Controllers creation
        _boardController = new BoardController(_boardSize.x, _boardSize.y);
        _playerController = new PlayerController(_userData);
        _itemController = new ItemController(_userData);

        //Init. controllers
        _playerController.Initialize();
        _itemController.Initialize();

        Instantiate(_boardViewPrefab).Initialize(_boardController, _boardSize);
        Instantiate(_playerViewPrefab, transform).Initialize(_playerController, _userData);
        Instantiate(_itemViewPrefab, transform).Initialize(_itemController, _userData);
        //Instantiate(_skillViewPrefab, transform).Initialize(_playerController, _userData);
        Instantiate(_enemyViewPrefab, transform);
        Instantiate(_topBar, transform);
        Instantiate(_sceneLoaderPrefab);
    }

}

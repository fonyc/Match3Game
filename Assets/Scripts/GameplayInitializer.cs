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
    [SerializeField] private SceneLoader _sceneLoaderPrefab = null;
    [SerializeField] private EnemyView _enemyViewPrefab = null;
    [SerializeField] private Gameplay_TopBar _topBar = null;


    private BoardController _boardController;
    private PlayerController _playerController;

    private UserData _userData;

    void Start()
    {
        //Controllers creation
        _userData = new UserData();
        _boardController = new BoardController(_boardSize.x, _boardSize.y);
        _playerController = new PlayerController(_userData);
        
        //Init. controllers
        _playerController.Initialize();
        
        Instantiate(_boardViewPrefab).Initialize(_boardController, _boardSize);

        Instantiate(_playerViewPrefab, transform).Initialize(_playerController, _userData);

        Instantiate(_enemyViewPrefab, transform);

        Instantiate(_topBar, transform);
    }

}

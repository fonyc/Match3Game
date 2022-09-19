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

    private BoardController _boardController;

    private UserData _userData;

    void Start()
    {
        _userData = new UserData();
        _boardController = new BoardController(_boardSize.x, _boardSize.y);
        
        Instantiate(_boardViewPrefab).Initiliaze(_boardController, _boardSize);

    }

}

using TMPro;
using UnityEngine;

public class RoundView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _currentMoves = null;

    IntArgument_Event _onMovesAvailableChanged;
    GameConfigService _gameConfigService;

    public int _dangerThreshold;

    public void Initialize(IntArgument_Event OnMovesAvailableChanged, GameConfigService gameConfigService)
    {
        _onMovesAvailableChanged = OnMovesAvailableChanged;
        _onMovesAvailableChanged.AddListener(UpdateMoves);
        _gameConfigService = gameConfigService;
        _dangerThreshold = _gameConfigService.dangerThreshold;
    }

    public void UpdateMoves(int moves)
    {
        _currentMoves.text = moves.ToString();
        _currentMoves.color = moves <= _dangerThreshold ? Color.red : Color.white;
    }
}

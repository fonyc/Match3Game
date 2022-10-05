using System.Collections.Generic;
using UnityEngine;

public class MatchView : MonoBehaviour
{
    private NoArgument_Event _onPlayerDeath;
    private NoArgument_Event _onEnemyDeath;

    private MatchController _matchController;

    [SerializeField] private GameObject _winPanelPrefab;
    [SerializeField] private GameObject _losePanelPrefab;
    [SerializeField] private GameObject _roundPanelPrefab; 

    IntArgument_Event _onMovesAvailableChanged;

    public void Initialize(MatchController matchController, 
        IntArgument_Event OnMovesAvailableChanged, NoArgument_Event OnPlayerDeath, 
        NoArgument_Event OnEnemyDeath)
    {
        _matchController = matchController;

        _onMovesAvailableChanged = OnMovesAvailableChanged;
        _onPlayerDeath = OnPlayerDeath;
        _onEnemyDeath = OnEnemyDeath;

        _onMovesAvailableChanged.AddListener(OnRoundOver);
        _onPlayerDeath.AddListener(OnPlayerLose);
        _onEnemyDeath.AddListener(OnPlayerWins);
    }

    private void OnDestroy()
    {
        _onPlayerDeath.RemoveListener(OnPlayerLose);
        _onEnemyDeath.RemoveListener(OnPlayerWins);
    }

    private void OnRoundOver(int moves)
    {
        if(moves == 0)
        {
            _roundPanelPrefab.SetActive(true);
            //Dragon perform attack
            //Open round resume
            Debug.Log("Match is over!");
        }
    }

    public void CloseroundPanel()
    {
        _roundPanelPrefab.SetActive(false);
    }

    private void OnPlayerWins()
    {
        _winPanelPrefab.SetActive(true);
        _matchController.GrantRewards();
    }

    private void OnPlayerLose()
    {
        _losePanelPrefab.SetActive(true);
    }

    public void GoBackToMainMenu()
    {
        _matchController.GoToMainMenu();
    }

    public void GrantRewards()
    {

    }

}

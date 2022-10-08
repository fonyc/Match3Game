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

    public void Initialize(MatchController matchController, NoArgument_Event OnPlayerDeath, NoArgument_Event OnEnemyDeath)
    {
        _matchController = matchController;

        _onPlayerDeath = OnPlayerDeath;
        _onEnemyDeath = OnEnemyDeath;

        //_onRoundIsOver.AddListener(OnRoundOver);
        _onPlayerDeath.AddListener(OnPlayerLose);
        _onEnemyDeath.AddListener(OnPlayerWins);
    }

    private void OnDestroy()
    {
        _onPlayerDeath.RemoveListener(OnPlayerLose);
        _onEnemyDeath.RemoveListener(OnPlayerWins);
        //_onRoundIsOver.RemoveListener(OnRoundOver);
    }

    private void OnRoundOver()
    {
        _roundPanelPrefab.SetActive(true);
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
}

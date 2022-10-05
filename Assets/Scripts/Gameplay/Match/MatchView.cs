using Shop.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchView : MonoBehaviour
{
    private NoArgument_Event _onPlayerDeath;
    private NoArgument_Event _onEnemyDeath;

    private MatchController _matchController;

    [SerializeField] private GameObject _winPanelPrefab;
    [SerializeField] private GameObject _losePanelPrefab;
    [SerializeField] private RoundReport _roundPanelPrefab; 

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
            //Dragon perform attack
            //Open round resume
            Debug.Log("Match is over!");
        }
    }

    private void OnPlayerWins()
    {
        //Instantiate(_winPanelPrefab);
        _matchController.GrantRewards();
    }

    private void OnPlayerLose()
    {
        //Instantiate(_losePanelPrefab);
    }

    public void GrantRewards(List<ResourceItem> rewards)
    {

    }

}

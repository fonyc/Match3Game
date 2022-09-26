using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{
    [SerializeField] private NoArgument_Event _OnBossDied;
    [SerializeField] private NoArgument_Event _OnPlayerDied;

    [SerializeField] GameObject defeatPanel;
    [SerializeField] GameObject victoryPanel;

    private void Awake()
    {
        _OnBossDied.AddListener(Victory);
        _OnPlayerDied.AddListener(Defeat);
    }

    public void Defeat()
    {
        defeatPanel.SetActive(true);
    }

    private void Victory()
    {
        victoryPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        _OnBossDied.RemoveListener(Victory);
        _OnPlayerDied.RemoveListener(Defeat);
    }
}

public enum GameStates
{
    Playing = 0,
    GameOver = 1
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NoArgument_Event _OnBossDied;
    [SerializeField] private NoArgument_Event _OnPlayerDied;

    private void Awake()
    {
        _OnBossDied.AddListener(GameOver);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        _OnBossDied.RemoveListener(GameOver);
    }
}

public enum GameStates
{
    Playing = 0,
    GameOver = 1
}

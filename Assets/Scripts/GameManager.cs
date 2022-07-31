using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;

    public static GameManager Instance { get => _instance; }

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }
}

public enum GameStates
{
    Playing = 0,
    GameOver = 1
}

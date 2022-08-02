using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;

    public static GameManager Instance { get => _instance; }

    [SerializeField] private EventBus _OnBossDied;
    [SerializeField] private EventBus _OnPlayerDied;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

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

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;

    [SerializeField] private TextMeshProUGUI enemyHP_txt;
    [SerializeField] private TextMeshProUGUI playerHP_txt;

    [SerializeField] RectTransform enemyFill;
    [SerializeField] RectTransform playerFill;

    public static UIManager Instance { get => _instance; }

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

    public void UpdatePlayerHealth(int hp, int max)
    {
        playerHP_txt.text = hp.ToString() + " / " + max.ToString();
        float scaleX = (float) hp / (float)max;
        playerFill.localScale = new Vector3(scaleX, playerFill.localScale.y, playerFill.localScale.z);
    }

    public void UpdateEnemyHealth(int hp, int max)
    {
        enemyHP_txt.text = hp.ToString() + " / " + max.ToString();
        float scaleX = (float)hp / (float)max;
        enemyFill.localScale = new Vector3(scaleX, playerFill.localScale.y, playerFill.localScale.z);
    }
}

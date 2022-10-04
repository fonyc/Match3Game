using Shop.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UserData _userData;

    private NoArgument_Event playerDeath;
    private NoArgument_Event enemyDeath;

    private GameObject winPanel;
    private GameObject losePanel;

    public void Initialize(UserData userData)
    {
        _userData = userData;

        playerDeath.AddListener(OnPlayerLose);
        playerDeath.AddListener(OnPlayerWins);
    }

    private void OnPlayerWins()
    {
        Instantiate(winPanel);
    }

    private void OnPlayerLose()
    {
        Instantiate(losePanel);
    }

    public void GrantRewards(List<ResourceItem> rewards)
    {

    }


}

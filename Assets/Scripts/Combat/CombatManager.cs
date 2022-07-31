using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private Hero hero;

    private Board board;

    private int heroCurrentHP;
    private int heroAttack;
    private int heroDefense;

    private int enemyCurrentHP;
    private int currentTurns;
    private int turnsToAttack;

    Dictionary<EmblemColor, int> attackReport = new();


    private void Start()
    {
        board = GetComponent<Board>();
        heroCurrentHP = hero.HP;
        heroAttack = hero.attack;
        heroDefense = hero.defense;

        enemyCurrentHP = enemy.HP;
        turnsToAttack = enemy.turnsToAttack;
        currentTurns = 0;

        UIManager.Instance.UpdateEnemyHealth(enemyCurrentHP, enemy.HP);
        UIManager.Instance.UpdatePlayerHealth(heroCurrentHP, hero.HP);
        UIManager.Instance.UpdateEnemyTurns(currentTurns, turnsToAttack);
    }

    public void UpdateEnemyTurns()
    {
        currentTurns++;
        UIManager.Instance.UpdateEnemyTurns(currentTurns, turnsToAttack);
        if(currentTurns == turnsToAttack)
        {
            AttackPlayer();
            currentTurns = 0;
            UIManager.Instance.UpdateEnemyTurns(currentTurns, turnsToAttack);
        }
    }

    private void AttackPlayer()
    {
        int dmg = enemy.attack - heroDefense;
        if (dmg > 0) heroCurrentHP = heroCurrentHP - dmg < 0 ? 0 : heroCurrentHP - dmg;

        UIManager.Instance.UpdatePlayerHealth(heroCurrentHP, hero.HP);


        if (CheckDeath(heroCurrentHP))
        {
            //Endgame. Player lose
            GameManager.Instance.GameOver();
            Debug.Log("Player Lose");
        }
    }

    private bool CheckDeath(int hp)
    {
        return hp == 0;
    }

    public void UpdateAttackReport(Dictionary<EmblemColor, int> newAttackReport)
    {
        attackReport = newAttackReport;

        EnemyRecieveDamage();
    }

    public void EnemyRecieveDamage()
    {
        foreach (KeyValuePair<EmblemColor, int> attack in attackReport)
        {
            int dmg = (int)Mathf.Round(heroAttack * attack.Value * TypeBonification(attack.Key, enemy.weaknesses, enemy.strengths));
            enemyCurrentHP = enemyCurrentHP - dmg < 0 ? 0 : enemyCurrentHP - dmg;

            UIManager.Instance.UpdateEnemyHealth(enemyCurrentHP, enemy.HP);

            //Debug.Log("Enemy Recieved " + attack.Value + " attacks of type " + attack.Key + "causing DMG: " + dmg + " and HP is now " + enemyCurrentHP);
            if(CheckDeath(enemyCurrentHP))
            {
                //Endgame player wins
                GameManager.Instance.GameOver();
                Debug.Log("Player Wins");
            }

        }
    }

    private float TypeBonification(EmblemColor attacker, List<EmblemColor> weaknesses, List<EmblemColor> strengths)
    {
        if (weaknesses.Contains(attacker) && !strengths.Contains(attacker)) return 2.0f;
        if (strengths.Contains(attacker) && !weaknesses.Contains(attacker)) return .5f;
        else return 1.0f;
    }
}

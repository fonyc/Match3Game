using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OldBoard))]
public class BattlegroundManager : MonoBehaviour
{
    [SerializeField] List<EnemyStats> enemyCreationList = new();

    [SerializeField] Enemy[] enemiesInColumns;
    [SerializeField] List<Enemy> enemyList;

    [SerializeField] HeroStats _heroStats;

    [SerializeField] private Hero _hero;

    OldBoard _board;

    public Enemy[] EnemiesInColumns { get => enemiesInColumns; set => enemiesInColumns = value; }
    public List<Enemy> EnemyList { get => enemyList; set => enemyList = value; }

    private void Awake()
    {
        EnemyList = new();
        _board = GetComponent<OldBoard>();
        EnemiesInColumns = new Enemy[_board.Width];
    }

    private void Start()
    {
        CreateBattleground();
    }

    private void InitHero()
    {
        if (_heroStats)
        {
            _hero = new Hero(_heroStats);
        }
    }

    private void InitEnemies()
    {
        if (EnemyList.Count <= 0) return;

        int index = 0;

        foreach (EnemyStats enemy in enemyCreationList)
        {
            int enemySize = enemy.size;

            if (enemySize <= 0 || index > EnemiesInColumns.Length)
            {
                Debug.LogError(enemy + "Enemy size in incorrect");
                return;
            }

            int sizeLeft = EnemiesInColumns.Length - index;

            if (enemySize > sizeLeft)
            {
                Debug.Log(enemy + "didn't fit. Board is full");
                continue;
            }

            Enemy newEnemy = new Enemy(enemy);
            EnemyList.Add(newEnemy);

            for (int x = index; x < enemySize; x++)
            {
                EnemiesInColumns[x] = newEnemy;
                index++;
            }
        }
    }

    private void CreateBattleground()
    {
        InitHero();
        InitEnemies();
    }
}

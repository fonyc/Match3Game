using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    #region VARIABLES
    [Header("--- PLAYER HERO ---")]
    [Space(5)]
    [SerializeField] private HeroStats2 Hero;
    private Hero _hero;

    [Header("--- ENEMY ---")]
    [Space(5)]
    [SerializeField] private EnemyStats Enemy;
    private Enemy _enemy;
    private OldBoard _board;

    //private BattlegroundManager _bgManager;

    List<Emblem> attackReport = new();

    public Hero HERO { get => _hero; set => _hero = value; }
    public Enemy ENEMY { get => _enemy; set => _enemy = value; }
    #endregion

    #region EVENTS
    [SerializeField] private NoArgument_Event _OnBossDied;
    [SerializeField] private NoArgument_Event _OnPlayerDied;

    [SerializeField] private DoubleIntArgument_Event _OnEnemyHealthChanged;
    [SerializeField] private IntArgument_Event _OnEnemyTurnsChanged;

    [SerializeField] private DoubleIntArgument_Event _OnPlayerHealthChanged;
    [SerializeField] private DoubleIntArgument_Event _OnPlayerCrossManaChanged;
    [SerializeField] private DoubleIntArgument_Event _OnPlayerVerticalManaChanged;
    [SerializeField] private DoubleIntArgument_Event _OnPlayerHorizontalManaChanged;

    [SerializeField] private BoolArgument_Event _OnCrossManaSkillReady;
    [SerializeField] private BoolArgument_Event _OnVerticalManaSkillReady;
    [SerializeField] private BoolArgument_Event _OnHorizontalManaSkillReady;
    #endregion

    private void Awake()
    {
        HERO = new Hero(Hero);
        ENEMY = new Enemy(Enemy);
        _board = GetComponent<OldBoard>();

        //_bgManager = GetComponent<BattlegroundManager>();
    }

    private void Start()
    {
        _OnPlayerHealthChanged.TriggerEvents(HERO.currentHP, HERO.HP);
        _OnEnemyHealthChanged.TriggerEvents(ENEMY.currentHP, ENEMY.HP);
        _OnEnemyTurnsChanged.TriggerEvents(ENEMY.currentTurns);
    }

    public void UpdateAttackReport(List<Emblem> newAttackReport)
    {
        attackReport = newAttackReport;

        EnemyRecieveDamage();
        GenerateManaWithAttack();
    }

    private void GenerateManaWithAttack()
    {
        int generatedCrossMana = 0;
        int generatedHorizontalMana = 0;
        int generatedVerticalMana = 0;

        foreach (Emblem emblem in attackReport)
        {
            if(emblem.OrientationAttack == OrientationAttack.Vertical)
            {
                generatedVerticalMana += HERO.verticalManaReg;
                generatedCrossMana += HERO.crossManaReg;
            }
            else if(emblem.OrientationAttack == OrientationAttack.Horizontal)
            {
                generatedHorizontalMana += HERO.horizontalManaReg;
                generatedCrossMana += HERO.crossManaReg;
            }
            else if (emblem.OrientationAttack == OrientationAttack.Cross)
            {
                generatedHorizontalMana += HERO.verticalManaReg;
                generatedVerticalMana += HERO.verticalManaReg;
                generatedCrossMana += HERO.crossManaReg;
            }
            else
            {
                continue;
            }
        }

        HERO.currentCrossMana = EnsureMaxStatCapacity(HERO.currentCrossMana, generatedCrossMana, HERO.crossMana);
        HERO.currentHorizontalMana = EnsureMaxStatCapacity(HERO.currentHorizontalMana, generatedHorizontalMana, HERO.horizontalMana);
        HERO.currentVerticalMana = EnsureMaxStatCapacity(HERO.currentVerticalMana, generatedVerticalMana, HERO.verticalMana);

        _OnPlayerCrossManaChanged.TriggerEvents(_hero.currentCrossMana, _hero.crossMana);
        _OnPlayerHorizontalManaChanged.TriggerEvents(_hero.currentHorizontalMana, _hero.horizontalMana);
        _OnPlayerVerticalManaChanged.TriggerEvents(_hero.currentVerticalMana, _hero.verticalMana);

        _OnCrossManaSkillReady.TriggerEvents(_hero.currentCrossMana == _hero.crossMana);
        _OnVerticalManaSkillReady.TriggerEvents(_hero.currentVerticalMana == _hero.verticalMana);
        _OnHorizontalManaSkillReady.TriggerEvents(_hero.currentHorizontalMana == _hero.horizontalMana);
    }

    private int EnsureMaxStatCapacity(int currentValue, int newValue, int maxValue)
    {
        return currentValue + newValue >= maxValue ? maxValue : currentValue + newValue;
    }

    private void EnemyRecieveDamage()
    {
        int dmg = 0;
        foreach (Emblem emblem in attackReport)
        {
            dmg += (int)Mathf.Round(
                HERO.attack * 
                TypeBonification(emblem.EmblemColor, ENEMY.colorWeaknesses, ENEMY.colorStrengths) *
                OrientationBonification(OrientationAttack.Vertical, OrientationAttack.Vertical));
        }

        ENEMY.currentHP = ENEMY.currentHP - dmg < 0 ? 0 : ENEMY.currentHP - dmg;
        if (CheckDeath(ENEMY.currentHP)) _OnBossDied.TriggerEvents();
        else UpdateEnemyTurns();
        _OnEnemyHealthChanged.TriggerEvents(ENEMY.currentHP, ENEMY.HP);
    }

    public void UpdateEnemyTurns()
    {
        if (ENEMY.currentTurns == 0)
        {
            AttackPlayer();
            ENEMY.currentTurns = ENEMY.turnsToAttack;
        }
        else ENEMY.currentTurns--;
        _OnEnemyTurnsChanged.TriggerEvents(ENEMY.currentTurns);
    }

    private void AttackPlayer()
    {
        int dmg = ENEMY.attack - HERO.defense;
        if (dmg > 0) HERO.currentHP = HERO.currentHP - dmg < 0 ? 0 : HERO.currentHP - dmg;

        _OnPlayerHealthChanged.TriggerEvents(HERO.currentHP, HERO.HP);

        if (CheckDeath(HERO.currentHP))
        {
            //Endgame. Player lose
            _OnPlayerDied.TriggerEvents();
            
        }
    }

    private bool CheckDeath(int hp)
    {
        return hp == 0;
    }

    private float TypeBonification(EmblemColor attacker, List<EmblemColor> weaknesses, List<EmblemColor> strengths)
    {
        if (weaknesses.Contains(attacker) && !strengths.Contains(attacker)) return 2.0f;
        if (strengths.Contains(attacker) && !weaknesses.Contains(attacker)) return .5f;
        else return 1.0f;
    }

    private float OrientationBonification(OrientationAttack attack, OrientationAttack defender)
    {
        if (attack == defender) return 1.0f;
        else return .5f;
    }
}

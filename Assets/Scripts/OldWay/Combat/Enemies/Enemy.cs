using System.Collections.Generic;

public class Enemy
{
    //STATS
    public EmblemColor emblemColor;
    public int HP;
    public int currentHP;

    public int attack;

    public int turnsToAttack;
    public int currentTurns;
    
    //Weaknesses
    public List<EmblemColor> colorWeaknesses = new();
    public List<EmblemColor> colorStrengths = new();    
    public OrientationAttack orientationWeakness;
    
    //BATTLEGROUND
    public int size;

    public Enemy(EnemyStats stats)
    {
        emblemColor = stats.emblemColor;
        HP = stats.HP;
        attack = stats.attack;
        turnsToAttack = stats.turnsToAttack;
        colorWeaknesses = stats.colorWeaknesses;
        colorStrengths = stats.colorStrengths;
        orientationWeakness = stats.orientationWeakness;
        size = stats.size;
        currentTurns = turnsToAttack;
        currentHP = HP;
    }
}

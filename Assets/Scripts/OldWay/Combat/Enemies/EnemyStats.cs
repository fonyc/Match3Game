using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Match3/Enemy")]
public class EnemyStats : ScriptableObject
{
    public EmblemColor emblemColor;
    public int HP;
    public int attack;
    public int turnsToAttack;
    public List<EmblemColor> colorWeaknesses = new();
    public List<EmblemColor> colorStrengths = new();
    public OrientationAttack orientationWeakness;
    public int size;
}
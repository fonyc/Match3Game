using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Match3/Enemy")]
public class Enemy : ScriptableObject
{
    public EmblemColor emblemColor;
    public int HP;
    public int attack;
    public int turnsToAttack;
    public List<EmblemColor> weaknesses = new();
    public List<EmblemColor> strengths = new();
}

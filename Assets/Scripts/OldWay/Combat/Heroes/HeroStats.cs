using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Match3/Hero")]
public class HeroStats2 : ScriptableObject
{
    [Header("--- BASE STATS ---")]
    [Space(5)]
    public int HP;
    public int attack;
    public int defense;

    [Header("--- WEAK/STR. ---")]
    [Space(5)]
    public EmblemColor emblemColor;
    public List<EmblemColor> weaknessList = new();
    public List<EmblemColor> strengthList = new();
    public OrientationAttack orientationWeakness;

    [Header("--- MANA ---")]
    [Space(5)]

    public int horizontalMana;
    public int verticalMana;
    public int crossMana;

    [Space(5)]
    public int horizontalManaReg;
    public int verticalManaReg;
    public int crossManaReg;
}

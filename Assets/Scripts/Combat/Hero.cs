using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Match3/Hero")]
public class Hero : ScriptableObject
{
    public EmblemColor emblemColor;
    public int HP;
    public int attack;
    public int defense;
}

using System.Collections.Generic;

public class Hero
{
    public int HP;
    public int currentHP;

    public int attack;
    public int defense;

    public EmblemColor emblemColor;
    public List<EmblemColor> weaknessList = new();
    public List<EmblemColor> strengthList = new();
    public OrientationAttack orientationWeakness;

    public int horizontalMana;
    public int verticalMana;
    public int crossMana;

    public int currentHorizontalMana;
    public int currentVerticalMana;
    public int currentCrossMana;

    public int horizontalManaReg;
    public int verticalManaReg;
    public int crossManaReg;

    public Hero(HeroStats2 heroStats)
    {
        HP = heroStats.HP;
        currentHP = HP;

        attack = heroStats.attack;
        defense = heroStats.defense;

        emblemColor = heroStats.emblemColor;
        weaknessList = heroStats.weaknessList;
        strengthList = heroStats.strengthList;
        orientationWeakness = heroStats.orientationWeakness;

        horizontalMana = heroStats.horizontalMana;
        verticalMana = heroStats.verticalMana;
        crossMana = heroStats.crossMana;

        currentHorizontalMana = 0;
        currentVerticalMana = 0;
        currentCrossMana = 0;

        horizontalManaReg = heroStats.horizontalManaReg;
        verticalManaReg = heroStats.verticalManaReg;
        crossManaReg = heroStats.crossManaReg;
    }
}

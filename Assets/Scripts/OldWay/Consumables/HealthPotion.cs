using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : IConsumable
{
    public int value = 30;
    public int qty = 3;

    public void Consume(Hero hero)
    {
        if (qty == 0) return;
        hero.currentHP = hero.currentHP + value >= hero.HP ? hero.HP : hero.currentHP + value;
        qty--;
    }
}

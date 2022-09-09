using UnityEngine;

public class VerticalManaPotion : IConsumable
{
    public int value = 50;
    public int qty = 3;

    public void Consume(Hero hero)
    {
        if (qty == 0 || hero.currentVerticalMana == hero.verticalMana) return;
        hero.currentVerticalMana = hero.currentVerticalMana + value >= hero.verticalMana ? hero.verticalMana : hero.currentVerticalMana + value;
        qty--;
    }
}

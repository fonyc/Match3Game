using UnityEngine;

public class CrossManaPotion : IConsumable
{
    public int value = 25;
    public int qty = 3;

    public void Consume(Hero hero)
    {
        if (qty == 0 || hero.currentCrossMana == hero.crossMana) return;
        hero.currentCrossMana = hero.currentCrossMana + value >= hero.crossMana ? hero.crossMana : hero.currentCrossMana + value;
        qty--;
    }
}

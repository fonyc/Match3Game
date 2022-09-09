using UnityEngine;

public class HorizontalManaPotion : IConsumable
{
    public int value = 50;
    public int qty = 3;

    public void Consume(Hero hero)
    {
        if (qty == 0 || hero.currentHorizontalMana == hero.horizontalMana) return;
        hero.currentHorizontalMana = hero.currentHorizontalMana + value >= hero.horizontalMana ? hero.horizontalMana : hero.currentHorizontalMana + value;
        qty--;
    }
}

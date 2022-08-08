using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableManager : MonoBehaviour
{
    CombatManager combatManager;

    [SerializeField] private IConsumable[] consumables = new IConsumable[3];

    [SerializeField] private DoubleIntArgument_Event _OnPlayerCrossManaChanged;
    [SerializeField] private DoubleIntArgument_Event _OnPlayerVerticalManaChanged;
    [SerializeField] private DoubleIntArgument_Event _OnPlayerHorizontalManaChanged;
    [SerializeField] private DoubleIntArgument_Event _OnPlayerHealthChanged;

    [SerializeField] private BoolArgument_Event _OnCrossManaSkillReady;
    [SerializeField] private BoolArgument_Event _OnVerticalManaSkillReady;
    [SerializeField] private BoolArgument_Event _OnHorizontalManaSkillReady;

    private void Start()
    {
        combatManager = GetComponent<CombatManager>();

        consumables[0] = new VerticalManaPotion();
        consumables[1] = new CrossManaPotion();
        consumables[2] = new HealthPotion();

        Hero hero = combatManager.HERO;
        _OnPlayerCrossManaChanged.TriggerEvents(hero.currentCrossMana, hero.crossMana);
        _OnPlayerHorizontalManaChanged.TriggerEvents(hero.currentHorizontalMana, hero.horizontalMana);
        _OnPlayerVerticalManaChanged.TriggerEvents(hero.currentVerticalMana, hero.verticalMana);
    }

    public void ConsumeItem(int x)
    {
        consumables[x].Consume(combatManager.HERO);

        //Update every possible field. Change this in the future
        Hero hero = combatManager.HERO;
        _OnPlayerCrossManaChanged.TriggerEvents(hero.currentCrossMana, hero.crossMana);
        _OnPlayerHorizontalManaChanged.TriggerEvents(hero.currentHorizontalMana, hero.horizontalMana);
        _OnPlayerVerticalManaChanged.TriggerEvents(hero.currentVerticalMana, hero.verticalMana);
        _OnPlayerHealthChanged.TriggerEvents(hero.currentHP, hero.HP);

        _OnCrossManaSkillReady.TriggerEvents(hero.currentCrossMana == hero.crossMana);
        _OnVerticalManaSkillReady.TriggerEvents(hero.currentVerticalMana == hero.verticalMana);
        _OnHorizontalManaSkillReady.TriggerEvents(hero.currentHorizontalMana == hero.horizontalMana);
    }
}

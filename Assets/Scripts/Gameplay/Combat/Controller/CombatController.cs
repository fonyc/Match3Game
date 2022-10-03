using Shop.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController
{
    public CombatModel Model;

    public void Initialize()
    {
        Load();
    }

    public void Load()
    {
        Model = JsonUtility.FromJson<CombatModel>(Resources.Load<TextAsset>("VulnerabilitiesModel").text);
    }

    public int RecieveAttack(int ATK, int DEF, int hits, int colorAttack, int colorDefense)
    {
        return Mathf.FloorToInt(hits * CalculateBonusPerHit(hits) * (CalculateAttackBonus(ATK, DEF)) * CalculateColorBonus(colorDefense, colorAttack));
    }

    private int CalculateAttackBonus(int ATK, int DEF)
    {
        if (DEF >= ATK) return 1;
        else return ATK - DEF;
    }

    private float CalculateColorBonus(int colorDefense, int colorAttack)
    {
        EmblemInteraction interactionSelected = null;

        foreach (EmblemInteraction interaction in Model.Vulnerabilities)
        {
            if (interaction.Color == colorAttack)
            {
                interactionSelected = interaction;
                break;
            }
        }

        if (interactionSelected.Strength.Color == colorDefense) return interactionSelected.Strength.Modificator;
        if (interactionSelected.Weakness.Color == colorDefense) return interactionSelected.Weakness.Modificator;

        //foreach (int strength in interactionSelected.Strength.Colors.ColorWrapper)
        //{
        //    if (strength == colorDefense)
        //        return interactionSelected.Strength.Modificator;
        //}

        //foreach (int weak in interactionSelected.Weakness.Colors.ColorWrapper)
        //{
        //    if (weak == colorDefense)
        //        return interactionSelected.Strength.Modificator;
        //}

        return interactionSelected.DefaultModificator;
    }

    private float CalculateBonusPerHit(int hits)
    {
        float result = 1;
        for (int x = 1; x < hits; x++)
        {
            result += 1f;
        }
        return result;
    }
}



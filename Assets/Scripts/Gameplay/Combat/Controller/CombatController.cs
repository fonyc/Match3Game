using UnityEngine;

public class CombatController
{
    public CombatModel Model;

    private GameConfigService _gameConfigService;

    public CombatController(GameConfigService gameConfigService)
    {
        _gameConfigService = gameConfigService;
    }

    public void Initialize()
    {
        Load();
    }

    public void Load()
    {
        Model = new CombatModel();
        Model.Vulnerabilities = _gameConfigService.VulnerabilitiesModel;
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



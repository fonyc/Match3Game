using Board.Controller;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Match/RoundData")]
public class MatchReport : ScriptableObject
{
    public int matchedEmblems;
    public int maxCombo;
    public int damageDealt;
    public int damageRecieved;
    public int columnsDestroyed;

    public void ClearReport()
    {
        matchedEmblems = 0;
        maxCombo = 0;
        damageDealt = 0;
        damageRecieved = 0;
        columnsDestroyed = 0;
    }

    public void AddMaxCombo(int combo)
    {
        maxCombo = Mathf.Max(combo, maxCombo);
    }

    public void AddDamage(int dmg)
    {
        damageDealt += dmg;
    }

    public void AddMatchedEmblems(int delta)
    {
        matchedEmblems += delta;
    }

    public int GetDestroyedColumns(BoardController board)
    {
        if (board.Model.Width <= columnsDestroyed) return 0;
        int count = 0;

        for (int x = 0; x < board.Model.Width - columnsDestroyed; x++)
        {
            if (board.Model.GetEmblem(x, 0).IsEmpty())
            {
                count++;
                columnsDestroyed++;
            }
        }
        return count;
    }
}

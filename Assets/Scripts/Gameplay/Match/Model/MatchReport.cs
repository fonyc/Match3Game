using Board.Controller;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Match/RoundData")]
public class MatchReport : ScriptableObject
{
    public int maxCombo;
    public int damageDealt;
    public int damageRecieved;
    public List<int> alreadyDestroyedColumns = new();

    public void ResetValues()
    {
        maxCombo = 0;
        damageDealt = 0;
        damageRecieved = 0;
        alreadyDestroyedColumns.Clear();
    }

    public void AddMaxCombo(int combo)
    {
        maxCombo = Mathf.Max(combo,maxCombo);
    }

    public int GetDestroyedColumns(BoardController board)
    {
        int count = 0;
        for (int x = 0; x < board.Model.Width; x++)
        {
            if (board.Model.GetEmblem(x, 0).IsEmpty() && !alreadyDestroyedColumns.Contains(x))
            {
                alreadyDestroyedColumns.Add(x);
                count++;
            }
        }
        return count;
    }


}

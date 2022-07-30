using System.Collections.Generic;
using UnityEngine;

public class VerticalSkill : ITileRemover
{
    public void RemoveEmblems(Board board, Emblem emblem)
    {
        Vector2Int posIndex = emblem.posIndex;

        List<Emblem> verticalEmblems = new();

        for (int row = 0; row < board.Width - 1; row++)
        {
            Emblem emblemToAdd = board.BoardStatus[row, emblem.posIndex.y];
            if (!verticalEmblems.Contains(emblemToAdd)) verticalEmblems.Add(emblemToAdd);
            Debug.Log(board.BoardStatus[row, emblem.posIndex.y]);
        }
    }
}
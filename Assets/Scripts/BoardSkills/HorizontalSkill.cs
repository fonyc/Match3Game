using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalSkill : ITileRemover
{
    public void RemoveEmblems(Board board, Emblem emblem)
    {
        Vector2Int posIndex = emblem.posIndex;

        List<Emblem> horizontalEmblems = new();

        for (int col = 0; col < board.Width - 1; col++)
        {
            Emblem emblemToAdd = board.BoardStatus[emblem.posIndex.x, col];
            if (!horizontalEmblems.Contains(emblemToAdd)) horizontalEmblems.Add(emblemToAdd);
            Debug.Log(board.BoardStatus[emblem.posIndex.x, col]);
        }
    }
}


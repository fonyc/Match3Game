using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSkill : ITileRemover
{
    public void RemoveEmblems(Board board, Emblem emblem)
    {
        Vector2Int posIndex = emblem.posIndex;

        List<Emblem> crossEmblems = new();

        for (int row = 0; row < board.Width - 1; row++)
        {
            Emblem emblemToAdd = board.BoardStatus[row, emblem.posIndex.y];
            if (!crossEmblems.Contains(emblemToAdd))crossEmblems.Add(emblemToAdd);
            Debug.Log(board.BoardStatus[row, emblem.posIndex.y]);
        }

        for (int col = 0; col < board.Width - 1; col++)
        {
            Emblem emblemToAdd = board.BoardStatus[emblem.posIndex.x, col];
            if (!crossEmblems.Contains(emblemToAdd)) crossEmblems.Add(emblemToAdd);
            Debug.Log(board.BoardStatus[emblem.posIndex.x, col]);
        }
    }
}
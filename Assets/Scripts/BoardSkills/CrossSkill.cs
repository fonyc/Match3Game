using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSkill : ITileRemover
{
    public List<Emblem> RemoveEmblems(Board board, Emblem emblem)
    {
        List<Emblem> crossEmblems = new();

        for (int x = 0; x < board.Width; x++)
        {
            Emblem emblemToAdd = board.BoardStatus[emblem.posIndex.x, x];
            if (!crossEmblems.Contains(emblemToAdd)) 
            {
                emblemToAdd.isMatched = true;
                crossEmblems.Add(emblemToAdd);
            }   
            
        }

        for (int x = 0; x < board.Height; x++)
        {
            Emblem emblemToAdd = board.BoardStatus[x, emblem.posIndex.y];
            if (!crossEmblems.Contains(emblemToAdd))
            {
                emblemToAdd.isMatched = true;
                crossEmblems.Add(emblemToAdd);
            }
        }
        return crossEmblems;
    }
}
using System.Collections.Generic;

public class HorizontalSkill : ITileRemover
{
    public List<Emblem> RemoveEmblems(Board board, Emblem emblem, Hero hero)
    {
        List<Emblem> horizontalEmblems = new();

        for (int x = 0; x < board.Height; x++)
        {
            Emblem emblemToAdd = board.BoardStatus[x, emblem.posIndex.y];
            if (!horizontalEmblems.Contains(emblemToAdd)) 
            {
                emblemToAdd.isMatched = true;
                horizontalEmblems.Add(emblemToAdd);
            }
        }
        hero.currentHorizontalMana = 0;
        return horizontalEmblems;
    }
}


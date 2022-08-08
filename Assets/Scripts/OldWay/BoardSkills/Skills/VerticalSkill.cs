using System.Collections.Generic;

public class VerticalSkill : ITileRemover
{
    public List<Emblem> RemoveEmblems(Board board, Emblem emblem, Hero hero)
    {
        List<Emblem> verticalEmblems = new();

        for (int x = 0; x < board.Width; x++)
        {
            Emblem emblemToAdd = board.BoardStatus[emblem.posIndex.x, x];
            if (!verticalEmblems.Contains(emblemToAdd)) 
            { 
                emblemToAdd.isMatched = true;
                verticalEmblems.Add(emblemToAdd);
            }
        }

        hero.currentVerticalMana = 0;
        return verticalEmblems;
    }
}
using System.Collections.Generic;

public interface ITileRemover
{
    List<Emblem> RemoveEmblems(Board board, Emblem emblem, Hero hero);
}
using System.Collections.Generic;

public interface ITileRemover
{
    List<Emblem> RemoveEmblems(OldBoard board, Emblem emblem, Hero hero);
}
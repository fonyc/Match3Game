using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    private Board board;

    [SerializeField] private List<Emblem> currentMatches = new();

    public List<Emblem> CurrentMatches { get => currentMatches; set => currentMatches = value; }

    private void Awake()
    {
        board = GetComponent<Board>();
    }

    public void FindAllMatches()
    {
        currentMatches.Clear();

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                Emblem currentEmblem = board.BoardStatus[x, y];
                if (currentEmblem == null) continue;

                //Horizontal axis limits
                if (HorizontalLimits(x, y))
                {
                    Emblem leftEmblem = board.BoardStatus[x - 1, y];
                    Emblem rightEmblem = board.BoardStatus[x + 1, y];

                    if (leftEmblem != null && rightEmblem != null)
                    {
                        if (HasSameEmblemType(leftEmblem, currentEmblem) && HasSameEmblemType(rightEmblem, currentEmblem))
                        {
                            currentEmblem.isMatched = true;
                            leftEmblem.isMatched = true;
                            rightEmblem.isMatched = true;

                            AddItemToList(currentEmblem, currentMatches);
                            AddItemToList(leftEmblem, currentMatches);
                            AddItemToList(rightEmblem, currentMatches);
                        }
                    }
                }

                if (VerticalLimits(x, y))
                {
                    Emblem upEmblem = board.BoardStatus[x, y + 1];
                    Emblem downEmblem = board.BoardStatus[x, y - 1];

                    if (upEmblem != null && downEmblem != null)
                    {
                        if (HasSameEmblemType(upEmblem, currentEmblem) && HasSameEmblemType(downEmblem, currentEmblem))
                        {
                            currentEmblem.isMatched = true;
                            upEmblem.isMatched = true;
                            downEmblem.isMatched = true;

                            AddItemToList(currentEmblem, currentMatches);
                            AddItemToList(upEmblem, currentMatches);
                            AddItemToList(downEmblem, currentMatches);
                        }
                    }
                }
            }
        }
    }

    #region UTILITY METHODS
    private bool HorizontalLimits(int x, int y)
    {
        return x > 0 && x < board.Width - 1;
    }

    private bool VerticalLimits(int x, int y)
    {
        return y > 0 && y < board.Height - 1;
    }

    private bool HasSameEmblemType(Emblem emblem1, Emblem emblem2)
    {
        return emblem1.EmblemType == emblem2.EmblemType;
    }

    private void AddItemToList(Emblem emblem, List<Emblem> list)
    {
        if (!list.Contains(emblem)) list.Add(emblem);
    }
    #endregion
}

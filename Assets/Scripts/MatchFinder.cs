using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    private Board board;
    private CombatManager combatManager;

    [SerializeField] private List<Emblem> currentMatches = new();

    public List<Emblem> CurrentMatches { get => currentMatches; set => currentMatches = value; }

    private void Awake()
    {
        board = GetComponent<Board>();
        combatManager = GetComponent<CombatManager>();
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

                if (HorizontalLimits(x, y))
                {
                    Emblem leftEmblem = board.BoardStatus[x - 1, y];
                    Emblem rightEmblem = board.BoardStatus[x + 1, y];

                    if (leftEmblem != null && rightEmblem != null)
                    {
                        if (HasSameEmblemColor(leftEmblem, currentEmblem) && HasSameEmblemColor(rightEmblem, currentEmblem))
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
                        if (HasSameEmblemColor(upEmblem, currentEmblem) && HasSameEmblemColor(downEmblem, currentEmblem))
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
        SendAttackReport();
    }

    #region BOOSTER RELATED

    public Dictionary<EmblemColor, List<Emblem>> GroupMatchesByEmblem(List<Emblem> matchList)
    {
        Dictionary<EmblemColor, List<Emblem>> matchDictionary = new();

        foreach (Emblem emblem in matchList)
        {
            if (!matchDictionary.ContainsKey(emblem.EmblemColor))
            {
                List<Emblem> emblemList = new();
                emblemList.Add(emblem);
                matchDictionary.Add(emblem.EmblemColor, emblemList);
            }
            else
            {
                List<Emblem> emblemList = matchDictionary[emblem.EmblemColor];
                if (!emblemList.Contains(emblem)) emblemList.Add(emblem);
            }
        }

        return matchDictionary;
    }
    #endregion

    #region UTILITY METHODS
    private bool HorizontalLimits(int x, int y)
    {
        return x > 0 && x < board.Width - 1;
    }

    private bool VerticalLimits(int x, int y)
    {
        return y > 0 && y < board.Height - 1;
    }

    private bool HasSameEmblemColor(Emblem emblem1, Emblem emblem2)
    {
        return emblem1.EmblemColor == emblem2.EmblemColor;
    }

    private void AddItemToList(Emblem emblem, List<Emblem> list)
    {
        if (!list.Contains(emblem)) list.Add(emblem);
    }
    #endregion

    #region COMBAT MANAGER RELATED

    private void SendAttackReport()
    {
        if (currentMatches.Count > 2)
        {
            Dictionary<EmblemColor, int> attackReport = new();

            foreach (Emblem emblem in currentMatches)
            {
                if (!attackReport.ContainsKey(emblem.EmblemColor))
                {
                    attackReport.Add(emblem.EmblemColor, 1);
                }
                else
                {
                    attackReport[emblem.EmblemColor] += 1;
                }
            }
            if (attackReport == null || attackReport.Count == 0) return;

            combatManager.UpdateAttackReport(attackReport);
        }

    }

    #endregion
}

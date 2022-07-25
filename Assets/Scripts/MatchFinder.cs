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
                        if (HasSameEmblemColor(leftEmblem, currentEmblem) && HasSameEmblemColor(rightEmblem, currentEmblem))
                        {
                            currentEmblem.isMatched = true;
                            leftEmblem.isMatched = true;
                            rightEmblem.isMatched = true;

                            AddItemToList(currentEmblem, currentMatches);
                            AddItemToList(leftEmblem, currentMatches);
                            AddItemToList(rightEmblem, currentMatches);

                            #region CROSS CHECK VERTICAL
                            //If the tile is a cross and its A MATCH, add the up and down tiles (nvm the color up or down)
                            if (currentEmblem.EmblemClass == EmblemClass.Cross)
                            {
                                //Add down emblem if not on bottom position
                                if (y > 0)
                                {
                                    Emblem downEmblem = board.BoardStatus[x, y - 1];
                                    if (downEmblem != null)
                                    {
                                        downEmblem.isMatched = true;
                                        AddItemToList(downEmblem, currentMatches);
                                    }
                                }
                                //Add upper emblem if not on top position
                                if (y < board.Height - 1)
                                {
                                    Emblem upEmblem = board.BoardStatus[x, y + 1];
                                    if (upEmblem != null)
                                    {
                                        upEmblem.isMatched = true;
                                        AddItemToList(upEmblem, currentMatches);
                                    }
                                }
                                #endregion

                            }

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

                            #region CROSS CHECK HORIZONTAL
                            //If the tile is a cross and its A MATCH, add the up and down tiles (nvm the color up or down)
                            if (currentEmblem.EmblemClass == EmblemClass.Cross)
                            {
                                //Add down emblem if not on bottom position
                                if (x > 0)
                                {
                                    Emblem leftEmblem = board.BoardStatus[x - 1, y];
                                    if (leftEmblem != null)
                                    {
                                        leftEmblem.isMatched = true;
                                        AddItemToList(leftEmblem, currentMatches);
                                    }
                                }
                                //Add upper emblem if not on top position
                                if (x < board.Width - 1)
                                {
                                    Emblem rightEmblem = board.BoardStatus[x + 1, y];
                                    if (rightEmblem != null)
                                    {
                                        rightEmblem.isMatched = true;
                                        AddItemToList(rightEmblem, currentMatches);
                                    }
                                }
                            }
                            #endregion

                        }
                    }
                }
            }
            //Checks if the board must spawn a booster
            //CheckIfBooster();
        }
    }
        private void CheckIfBooster()
        {
            if (currentMatches.Count == 4)
            {
                int emblemColor = (int)currentMatches[0].EmblemColor;
                board.boosterToSpawn = board.crossDB[emblemColor];
            }
            else if (currentMatches.Count > 4)
            {
                //int emblemColor = (int)currentMatches[0].EmblemColor;
                //board.boosterToSpawn = board.crossDB[emblemColor];
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

        private bool HasSameEmblemColor(Emblem emblem1, Emblem emblem2)
        {
            return emblem1.EmblemColor == emblem2.EmblemColor;
        }

        private void AddItemToList(Emblem emblem, List<Emblem> list)
        {
            if (!list.Contains(emblem)) list.Add(emblem);
        }
        #endregion
    }

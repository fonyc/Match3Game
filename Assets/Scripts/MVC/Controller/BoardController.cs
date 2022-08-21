using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC.Model;
using System;
using Random = UnityEngine.Random;
using System.Linq;

namespace MVC.Controller
{
    public class BoardController
    {
        private BoardModel Model;

        //Events 
        public event Action<EmblemModel, EmblemModel> OnEmblemMoved = delegate (EmblemModel origin, EmblemModel destination) { };
        public event Action<EmblemModel, EmblemModel> OnEmblemColapse = delegate (EmblemModel origin, EmblemModel destination) { };
        public event Action<EmblemModel, EmblemModel> OnWrongEmblemMoved = delegate (EmblemModel origin, EmblemModel destination) { };
        public event Action<EmblemModel> OnEmblemDestroyed = delegate (EmblemModel emblemDestroyed) { };
        public event Action<EmblemModel, EmblemItem> OnEmblemCreated = delegate (EmblemModel emblemDestroyed, EmblemItem item) { };

        public BoardController(int width, int heigth, EmblemItem[,] initValues = null)
        {
            Model = new BoardModel(width, heigth, initValues);
            VerifyBoard();
        }

        private void VerifyBoard()
        {
            for (int y = 0; y < Model.Height; y++)
            {
                for (int x = 0; x < Model.Width; x++)
                {
                    while (FindMatchesAt(new Vector2Int(x, y)))
                    {
                        Model.GetEmblem(x, y).Item.EmblemColor = (EmblemColor)Random.Range(0, 5);
                    }
                }
            }
        }

        //This method requires the board to iterate first y then x and have 5 or more items(FIX)
        private bool FindMatchesAt(Vector2Int pos)
        {
            if (pos.x < Model.Width - 2)
            {
                if (HasSameColor(Model.GetEmblem(pos.x + 1, pos.y), Model.GetEmblem(pos.x, pos.y))
                    && HasSameColor(Model.GetEmblem(pos.x + 2, pos.y), Model.GetEmblem(pos.x, pos.y)))
                {
                    return true;
                }
            }

            if (pos.y < Model.Height - 2)
            {
                if (HasSameColor(Model.GetEmblem(pos.x, pos.y + 1), Model.GetEmblem(pos.x, pos.y))
                    && HasSameColor(Model.GetEmblem(pos.x, pos.y + 2), Model.GetEmblem(pos.x, pos.y)))
                {
                    return true;
                }
            }
            return false;
        }

        //BAD METHOD --> remove when we use a proper instantiation of views(FIX)
        public int GetEmblemColor(int x, int y)
        {
            return (int)Model.GetEmblem(x, y).Item.EmblemColor;
        }

        public void CheckInput(Vector2Int touchPosition, Vector2Int releasePosition)
        {
            if (TouchIsWithinLimits(touchPosition) && TouchIsWithinLimits(releasePosition))
            {
                ProcessMatch(touchPosition, releasePosition);
            }
        }

        private void ProcessMatch(Vector2Int touchPosition, Vector2Int releasePosition)
        {
            EmblemModel originEmblem = Model.GetEmblem(touchPosition);
            EmblemModel destinationEmblem = Model.GetEmblem(releasePosition);

            if (originEmblem.IsEmpty() || destinationEmblem.IsEmpty()) return;

            //Make the model changes to check the board
            SwapModelEmblems(originEmblem, destinationEmblem);

            List<EmblemModel> swapMatches = FindAllMatches();

            if (swapMatches.Count == 0)
            {
                //Undo the model changes
                SwapModelEmblems(originEmblem, destinationEmblem);

                //Make a Go and Back animation
                OnWrongEmblemMoved(originEmblem, destinationEmblem);
                return;
            }
            else
            {
                OnEmblemMoved(originEmblem, destinationEmblem);

                foreach (EmblemModel emblem in swapMatches)
                {
                    //Destroy View
                    OnEmblemDestroyed(emblem);
                    //Destroy model item
                    Model.GetEmblem(emblem.Position).Item = null;
                }

                //ColapseColumns();
                ProcessCollapse();

                //do { ColapseColumns(); }
                //while (FindAllMatches().Count != 0);
            }
        }

        private void ProcessCollapse()
        {
            for (int y = 0; y < Model.Height; ++y)
            {
                for (int x = 0; x < Model.Width; ++x)
                {
                    if (!Model.GetEmblem(x, y).IsEmpty())
                        continue;

                    int nextY = y;
                    while (nextY < Model.Height)
                    {
                        nextY++;
                        if (nextY == Model.Height)
                        {
                            Model.GetEmblem(x, nextY - 1).Item = new EmblemItem()
                            {
                                EmblemColor = (EmblemColor)Random.Range(0, 5)
                            };
                            OnEmblemCreated(Model.GetEmblem(x, nextY - 1), Model.GetEmblem(x, nextY - 1).Item);
                            if (y < nextY - 1)
                            {
                                Model.GetEmblem(x, y).Item = Model.GetEmblem(x, nextY - 1).Item;
                                Model.GetEmblem(x, nextY - 1).Item = null;
                                OnEmblemColapse(Model.GetEmblem(x, nextY - 1), Model.GetEmblem(x, y));
                            }

                            break;
                        }

                        if (!Model.GetEmblem(x, nextY).IsEmpty())
                        {
                            Model.GetEmblem(x, y).Item = Model.GetEmblem(x, nextY).Item;
                            Model.GetEmblem(x, nextY).Item = null;
                            OnEmblemColapse(Model.GetEmblem(x, nextY), Model.GetEmblem(x, y));
                            break;
                        }
                    }
                }
            }
        }

        private void ColapseColumns()
        {
            int nullCounter = 0;

            for (int x = 0; x < Model.Width; x++)
            {
                for (int y = 0; y < Model.Height; y++)
                {
                    //EMBLEM DISPLACEMENT
                    if (Model.GetEmblem(x, y).IsEmpty())
                    {
                        nullCounter++;
                    }
                    else if (nullCounter > 0)
                    {
                        //Update Model
                        Model.GetEmblem(x, y).Item = Model.GetEmblem(x, y - nullCounter).Item;

                        //Update View
                        OnEmblemColapse(Model.GetEmblem(x, y), Model.GetEmblem(x, y - nullCounter));
                    }

                    //EMBLEM CREATION
                    if (y == Model.Height - 1 && nullCounter > 0)
                    {
                        for (int destroyCol = Model.Height - nullCounter; destroyCol < Model.Height; destroyCol++)
                        {
                            Model.GetEmblem(x, destroyCol).Item = new EmblemItem()
                            {
                                EmblemColor = (EmblemColor)Random.Range(0, 5)
                            };
                            OnEmblemCreated(Model.GetEmblem(x, destroyCol), Model.GetEmblem(x, destroyCol).Item);
                        }
                    }
                }
                nullCounter = 0;
            }
        }

        //Find matches in the whole board
        private List<EmblemModel> FindAllMatches()
        {
            List<EmblemModel> currentMatches = new();

            for (int y = 0; y < Model.Height; y++)
            {
                for (int x = 0; x < Model.Width; x++)
                {
                    EmblemModel emblem = Model.GetEmblem(x, y);

                    if (emblem.IsEmpty()) continue;

                    //Horizontal Limits
                    if (emblem.Position.x < Model.Width - 1 && emblem.Position.x > 0)
                    {
                        EmblemModel leftEmblem = Model.GetEmblem(emblem.Position.x - 1, emblem.Position.y);
                        EmblemModel rightEmblem = Model.GetEmblem(emblem.Position.x + 1, emblem.Position.y);

                        if (!leftEmblem.IsEmpty() && !rightEmblem.IsEmpty())
                        {
                            if (HasSameColor(leftEmblem, emblem) && HasSameColor(rightEmblem, emblem))
                            {
                                //Add orientation attack here in the future
                                currentMatches.Add(emblem);
                                currentMatches.Add(leftEmblem);
                                currentMatches.Add(rightEmblem);
                            }
                        }
                    }

                    //Vertical Limits
                    if (emblem.Position.y < Model.Height - 1 && emblem.Position.y > 0)
                    {
                        EmblemModel upEmblem = Model.GetEmblem(emblem.Position.x, emblem.Position.y + 1);
                        EmblemModel downEmblem = Model.GetEmblem(emblem.Position.x, emblem.Position.y - 1);

                        if (!upEmblem.IsEmpty() && !downEmblem.IsEmpty())
                        {
                            if (HasSameColor(upEmblem, emblem) && HasSameColor(downEmblem, emblem))
                            {
                                //Add orientation attack here in the future
                                currentMatches.Add(emblem);
                                currentMatches.Add(upEmblem);
                                currentMatches.Add(downEmblem);
                            }
                        }
                    }
                }
            }
            currentMatches = currentMatches.Distinct().ToList();
            return currentMatches;
        }

        #region UTILITY METHODS
        private void SwapModelEmblems(EmblemModel originEmblem, EmblemModel destinationEmblem)
        {
            EmblemItem originItem = originEmblem.Item;
            originEmblem.Item = destinationEmblem.Item;
            destinationEmblem.Item = originItem;
        }
        
        private bool HasSameColor(EmblemModel emblem1, EmblemModel emblem2)
        {
            return emblem1.Item.EmblemColor == emblem2.Item.EmblemColor;
        }

        private bool TouchIsWithinLimits(Vector2Int touchPosition)
        {
            return (touchPosition.x >= 0 &&
                touchPosition.y >= 0 &&
                touchPosition.y < Model.Height &&
                touchPosition.x < Model.Width);
        }
        #endregion
    }
}

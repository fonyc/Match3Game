using System.Collections.Generic;
using UnityEngine;
using Board.Model;
using System;
using Random = UnityEngine.Random;
using System.Linq;

namespace Board.Controller
{
    public class BoardController
    {
        private BoardModel Model;

        //Events 
        public event Action<Vector2Int, Vector2Int> OnEmblemMoved = delegate (Vector2Int origin, Vector2Int destination) { };
        public event Action<Vector2Int> OnEmblemDestroyed = delegate (Vector2Int emblemDestroyed) { };
        public event Action<Vector2Int, EmblemItem> OnEmblemCreated = delegate (Vector2Int emblemPosition, EmblemItem item) { };

        public BoardController(int width, int height, EmblemItem[,] initValues = null)
        {
            Model = new BoardModel(width, height, initValues);
        }

        public int GetEmblemColor(int x, int y)
        {
            return (int)Model.GetEmblem(x, y).Item.EmblemColor;
        }

        public void CheckInput(Vector2Int touchPosition)
        {
            if (TouchIsWithinLimits(touchPosition))
            {
                ProcessMatch(touchPosition);
            }
        }

        private void ProcessMatch(Vector2Int touchPosition)
        {
            EmblemModel touchedEmblem = Model.GetEmblem(touchPosition);

            if (touchedEmblem.IsEmpty()) return;

            List<EmblemModel> swapMatches = RecursiveSearch(touchedEmblem);

            if (swapMatches.Count > 1)
            {
                DestroyAndCollapse(swapMatches);
            }
        }

        private void DestroyAndCollapse(List<EmblemModel> comboMatches)
        {
            foreach (EmblemModel emblem in comboMatches)
            {
                //Destroy model item
                Model.GetEmblem(emblem.Position).Item = null;

                //Destroy View
                OnEmblemDestroyed(emblem.Position);
            }
            VerticalCollapse();
            HorizontalCollapse();

            List<EmblemModel> matchesLeft = CalculateEmblemsWithMatch();
            Debug.Log($"Matches left: {matchesLeft.Count}");
            if (matchesLeft.Count == 0)
            {
                Debug.Log("Match is over");
                RefillBoard();
            }
            else
            {
                Debug.Log("Keep playing");
            }
        }

        private List<EmblemModel> CalculateEmblemsWithMatch()
        {
            List<EmblemModel> moves = new();

            for (int x = 0; x < Model.Width; x++)
            {
                for (int y = 0; y < Model.Height; y++)
                {
                    if (Model.GetEmblem(x, y).IsEmpty()) continue;
                    if (x < Model.Width - 1 && HasSameColor(Model.GetEmblem(x, y), Model.GetEmblem(x + 1, y)))
                    {
                        moves.Add(Model.GetEmblem(x, y));
                        moves.Add(Model.GetEmblem(x + 1, y));
                    }

                    if (y < Model.Height - 1 && HasSameColor(Model.GetEmblem(x, y), Model.GetEmblem(x, y + 1)))
                    {
                        moves.Add(Model.GetEmblem(x, y));
                        moves.Add(Model.GetEmblem(x, y + 1));
                    }
                }
            }
            moves.Distinct().ToList();
            return moves;
        }

        private void VerticalCollapse()
        {
            int nullCounter = 0;

            for (int x = 0; x < Model.Width; x++)
            {
                for (int y = 0; y < Model.Height; y++)
                {
                    if (Model.GetEmblem(x, y).IsEmpty())
                    {
                        nullCounter++;
                    }
                    else if (nullCounter > 0)
                    {
                        //Switch emblems in model 
                        EmblemItem aux = Model.GetEmblem(x, y - nullCounter).Item;
                        Model.GetEmblem(x, y - nullCounter).Item = Model.GetEmblem(x, y).Item;
                        Model.GetEmblem(x, y).Item = aux;

                        //Send view to new position
                        OnEmblemMoved(Model.GetEmblem(x, y).Position, Model.GetEmblem(x, y - nullCounter).Position);
                    }
                }
                nullCounter = 0;
            }
        }

        private void HorizontalCollapse()
        {
            int nullCounter = 0;
            if (BoardIsSeparated())
            {
                nullCounter = 0;
                for (int x = 0; x < Model.Width; x++)
                {
                    if (Model.GetEmblem(x, 0).IsEmpty())
                    {
                        nullCounter++;
                        continue;
                    }
                    else if (nullCounter > 0)
                    {
                        for (int y = 0; y < Model.Height; y++)
                        {
                            //Switch emblems in model 
                            EmblemItem aux = Model.GetEmblem(x - nullCounter, y).Item;
                            Model.GetEmblem(x - nullCounter, y).Item = Model.GetEmblem(x, y).Item;
                            Model.GetEmblem(x, y).Item = aux;

                            //Send view to new position
                            OnEmblemMoved(Model.GetEmblem(x, y).Position, Model.GetEmblem(x - nullCounter, y).Position);
                        }
                    }
                }
            }
        }

        public void RefillBoard()
        {
            for (int x = 0; x < Model.Width; x++)
            {
                for (int y = 0; y < Model.Height; y++)
                {
                    if (!Model.GetEmblem(x, y).IsEmpty()) continue;

                    //Create model emblem
                    Model.GetEmblem(x, y).Item = new EmblemItem()
                    {
                        EmblemColor = (EmblemColor)Random.Range(0, 5)
                    };

                    //Createm view emblem
                    OnEmblemCreated(Model.GetEmblem(x, y).Position, Model.GetEmblem(x, y).Item);

                }
            }
        }

        private bool BoardIsSeparated()
        {
            for (int x = 0; x < Model.Width; x++)
            {
                if (Model.GetEmblem(x, 0).IsEmpty()) return true;
            }
            return false;
        }

        private List<EmblemModel> RecursiveSearch(EmblemModel currentEmblem, List<EmblemModel> exclude = null)
        {
            List<EmblemModel> result = new List<EmblemModel> { currentEmblem };

            if (exclude == null)
            {
                exclude = new List<EmblemModel> { currentEmblem };
            }
            else
            {
                if (!exclude.Contains(currentEmblem)) exclude.Add(currentEmblem);
            }

            foreach (EmblemModel neighbour in GetNeighbours(currentEmblem))
            {
                if (neighbour.Item == null || exclude.Contains(neighbour) || neighbour.Item.EmblemColor != currentEmblem.Item.EmblemColor) continue;
                result.AddRange(RecursiveSearch(neighbour, exclude));
            }
            return result;
        }

        private List<EmblemModel> GetNeighbours(EmblemModel emblem)
        {
            List<EmblemModel> neighbourList = new();

            //Up
            if (emblem.Position.y < Model.Height - 1) neighbourList.Add(Model.GetEmblem(emblem.Position.x, emblem.Position.y + 1));

            //Down
            if (emblem.Position.y > 0) neighbourList.Add(Model.GetEmblem(emblem.Position.x, emblem.Position.y - 1));

            //Left
            if (emblem.Position.x > 0) neighbourList.Add(Model.GetEmblem(emblem.Position.x - 1, emblem.Position.y));

            //Right
            if (emblem.Position.x < Model.Width - 1) neighbourList.Add(Model.GetEmblem(emblem.Position.x + 1, emblem.Position.y));

            return neighbourList;
        }

        #region UTILITY METHODS

        private bool HasSameColor(EmblemModel emblem1, EmblemModel emblem2)
        {
            if (emblem1.IsEmpty() || emblem2.IsEmpty()) return false;
            else return emblem1.Item.EmblemColor == emblem2.Item.EmblemColor;
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

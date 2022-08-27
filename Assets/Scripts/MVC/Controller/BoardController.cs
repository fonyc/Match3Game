using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC.Model;
using System;
using Random = UnityEngine.Random;

namespace MVC.Controller
{
    public class BoardController
    {
        private BoardModel Model;

        private List<int> itemsLeftInColumn;

        //Events 
        public event Action<EmblemModel, EmblemModel> OnEmblemMoved = delegate (EmblemModel origin, EmblemModel destination) { };
        public event Action<EmblemModel> OnEmblemDestroyed = delegate (EmblemModel emblemDestroyed) { };
        public event Action<EmblemModel, EmblemItem> OnEmblemCreated = delegate (EmblemModel emblemDestroyed, EmblemItem item) { };

        public BoardController(int width, int height, EmblemItem[,] initValues = null)
        {
            Model = new BoardModel(width, height, initValues);
            itemsLeftInColumn = new();
            FillColumnCounter(height);
        }

        public void FillColumnCounter(int height)
        {
            for (int x = 0; x < height; x++)
            {
                itemsLeftInColumn.Add(height);
            }
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
                itemsLeftInColumn[emblem.Position.y]--;

                //Destroy View
                OnEmblemDestroyed(emblem);
            }
            ProcessCollapse();
        }

        private void ProcessCollapse()
        {
            int nullCounter = 0;

            //Vertical Collapse
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
                        OnEmblemMoved(Model.GetEmblem(x, y), Model.GetEmblem(x, y - nullCounter));
                    }
                    
                    //FUTURE ITEM CREATION
                    //if (y == Model.Height - 1 && nullCounter > 0)
                    //{
                        //Item creation
                        //for (int fill = Model.Height - nullCounter; fill < Model.Height; fill++)
                        //{
                        //    //Create model emblem
                        //    Model.GetEmblem(x, fill).Item = new EmblemItem()
                        //    {
                        //        EmblemColor = (EmblemColor)Random.Range(0, 5)
                        //    };

                        //    //Createm view emblem
                        //    OnEmblemCreated(Model.GetEmblem(x, fill), Model.GetEmblem(x, fill).Item);
                        //}
                    //}
                }
                nullCounter = 0;
            }

            //Horizontal Collapse
            if (BoardIsSeparated())
            {
                //Means there are null columns separating items 

            }
        }

        private bool BoardIsSeparated()
        {
            //foreach (int col )
            //{

            //}
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC.Model;

namespace MVC.Controller
{
    public class BoardController
    {
        private BoardModel Model;

        //Events 
        //OnCellCreated, OnCellDestroyed, OnCellMoved ??

        //The controller constructor just passes the info to the model, so the model can be created with the view info
        public BoardController(int width, int heigth, EmblemItem[,] initValues = null)
        {
            Model = new BoardModel(width, heigth, initValues);
        }

        public void CheckInput(Vector2Int touchPosition)
        {
            if (TouchIsWithinLimits(touchPosition)) ProcessMatch(touchPosition);
        }

        private bool TouchIsWithinLimits(Vector2Int touchPosition)
        {
            return (touchPosition.x >= 0 && touchPosition.y >= 0 && touchPosition.y < Model.Height && touchPosition.x < Model.Width);
        }

        private void ProcessMatch(Vector2Int screenPosition)
        {
            //Transform the screen position into a board position
            EmblemModel touchedEmblem = Model.GetEmblem(screenPosition);

            //Ensure item is not null
            if (touchedEmblem.Item == null) return;

            //Get matches
            List<EmblemModel> matchedList = FindAllMatches(touchedEmblem, new List<int>());

            if (matchedList.Count <= 2) return;

            foreach (EmblemModel emblem in matchedList)
            {

            }
        }

        /// <summary>
        /// Open/close Fran's algorythm. 
        /// </summary>
        /// <param name="touchedCell"></param>
        /// <param name="extraAllowedMatches"></param>
        /// <returns></returns>
        private List<EmblemModel> FindAllMatches(EmblemModel touchedCell, List<int> extraAllowedMatches)
        {
            List<EmblemModel> closed = new List<EmblemModel>();
            if (touchedCell.IsEmpty())
                return closed;

            List<EmblemModel> open = new List<EmblemModel>();
            open.Add(touchedCell);

            while (open.Count > 0)
            {
                EmblemModel EmblemModel = open[0];
                open.RemoveAt(0);
                closed.Add(EmblemModel);

                if (EmblemModel.Position.x > 0)
                {
                    EmblemModel neighbour = Model.GetEmblem(EmblemModel.Position.x - 1, EmblemModel.Position.y);
                    CheckNeighbour(touchedCell, neighbour, extraAllowedMatches, open, closed);
                }

                if (EmblemModel.Position.x < Model.Width - 1)
                {
                    EmblemModel neighbour = Model.GetEmblem(EmblemModel.Position.x + 1, EmblemModel.Position.y);
                    CheckNeighbour(touchedCell, neighbour, extraAllowedMatches, open, closed);
                }

                if (EmblemModel.Position.y > 0)
                {
                    EmblemModel neighbour = Model.GetEmblem(EmblemModel.Position.x, EmblemModel.Position.y - 1);
                    CheckNeighbour(touchedCell, neighbour, extraAllowedMatches, open, closed);
                }

                if (EmblemModel.Position.y < Model.Height - 1)
                {
                    EmblemModel neighbour = Model.GetEmblem(EmblemModel.Position.x, EmblemModel.Position.y + 1);
                    CheckNeighbour(touchedCell, neighbour, extraAllowedMatches, open, closed);
                }
            }

            return closed;
        }

        private void CheckNeighbour(EmblemModel touchedCell, EmblemModel neighbour, List<int> extraAllowedMatches, List<EmblemModel> open, List<EmblemModel> closed)
        {
            if (!neighbour.IsEmpty() &&
                (touchedCell.Item.EmblemColor == neighbour.Item.EmblemColor ||
                extraAllowedMatches.Contains((int)neighbour.Item.EmblemColor)) &&
                !open.Contains(neighbour) && !closed.Contains(neighbour))
            {
                open.Add(neighbour);
            }
        }
    }
}

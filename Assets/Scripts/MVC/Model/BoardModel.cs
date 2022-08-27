using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVC.Model
{
    public class BoardModel
    {
        private EmblemModel[,] _boardStatus;

        public int Width { get; }
        public int Height { get; }

        #region CONSTRUCTORS

        public BoardModel(int width, int height, EmblemItem[,] initialValues = null)
        {
            Width = width;
            Height = height;

            _boardStatus = new EmblemModel[width, height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _boardStatus[x, y] = new EmblemModel
                    {
                        Position = new Vector2Int(x, y),
                        //Item = initialValues?[x, y]
                        Item = new EmblemItem { EmblemColor = (EmblemColor)Random.Range(0, 5) }
                    };
                }
            }
        }

        #endregion

        public EmblemModel GetEmblem(int x, int y) => _boardStatus[x, y];

        public EmblemModel GetEmblem(Vector2Int pos) => _boardStatus[pos.x, pos.y];

        public EmblemModel[,] BoardStatus()
        {
            return _boardStatus;
        }

        public void Clear()
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    _boardStatus[x, y] = new EmblemModel
                    {
                        Position = new Vector2Int(x, y),
                        Item = null
                    };
                }
            }
        }
    }
}
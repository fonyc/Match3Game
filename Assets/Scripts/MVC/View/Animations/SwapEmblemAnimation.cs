using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVC.View
{
    public class SwapEmblemAnimation : IViewAnimation
    {
        private Vector2Int _origin; 
        private Vector2Int _destination;

        public SwapEmblemAnimation(Vector2Int origin, Vector2Int destination)
        {
            _origin = origin;
            _destination = destination;
        }

        public Coroutine PlayAnimation(BoardView board)
        {
            return board.StartCoroutine(AnimationCoroutine(board));
        }

        IEnumerator AnimationCoroutine(BoardView board)
        {
            EmblemView origin = board.GetEmblemViewAtPosition(_origin);
            EmblemView destination = board.GetEmblemViewAtPosition(_destination);

            if (origin == null || destination == null) yield break;
            
            origin.MoveTo(_destination);
            yield return destination.MoveTo(_origin);
        }
    }
}
using System.Collections;
using UnityEngine;

namespace MVC.View
{
    public class ColapseEmblemAnimation : IViewAnimation
    {
        private Vector2Int _origin;
        private Vector2Int _destination;

        public ColapseEmblemAnimation(Vector2Int origin, Vector2Int destination)
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

            if (origin == null) yield break;

            origin.MoveTo(_destination);
        }
    }
}
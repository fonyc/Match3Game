using System.Collections;
using UnityEngine;

namespace MVC.View
{
    public class WrongSwapEmblemAnimation : IViewAnimation
    {
        private Vector2Int _origin, _destination;

        public WrongSwapEmblemAnimation(Vector2Int origin, Vector2Int destination)
        {
            _origin = origin;
            _destination= destination;
        }

        public Coroutine PlayAnimation(BoardView board)
        {
            return board.StartCoroutine(Animation_Coro(board));
        }

        IEnumerator Animation_Coro(BoardView board)
        {
            EmblemView origin = board.GetEmblemViewAtPosition(_origin);
            EmblemView destination = board.GetEmblemViewAtPosition(_destination);
            if (origin == null || destination == null)
                yield break;

            origin.MoveTo(_destination);
            yield return destination.MoveTo(_origin);

            yield return new WaitForSeconds(0.15f);

            origin.MoveTo(_origin);
            yield return destination.MoveTo(_destination);
        }
    }

}


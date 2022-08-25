using System.Collections;
using UnityEngine;

namespace MVC.View
{
    public class DestroyEmblemAnimation : IViewAnimation
    {
        private EmblemView _emblem;

        public DestroyEmblemAnimation(EmblemView emblem)
        {
            _emblem = emblem;
        }

        public Coroutine PlayAnimation(BoardView board)
        {
            return board.StartCoroutine(AnimationCoroutine(board));
        }

        IEnumerator AnimationCoroutine(BoardView board)
        {
            if (_emblem == null) yield break;
            board.RemoveEmblemView(_emblem);
            yield return _emblem.DestroyTile();
        }
    }
}

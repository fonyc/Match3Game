using Board.Model;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Board.View
{
    public class DestroyEmblemAnimation : IViewAnimation
    {
        private EmblemView _emblem;
        private int _color;

        public DestroyEmblemAnimation(EmblemView emblem, int color)
        {
            _emblem = emblem;
            _color = color;
        }

        public Coroutine PlayAnimation(BoardView board)
        {
            return board.StartCoroutine(AnimationCoroutine(board, _color));
        }

        IEnumerator AnimationCoroutine(BoardView board, int color)
        {
            if (_emblem == null) yield break;
            AsyncOperationHandle<GameObject> handler =
            Addressables.InstantiateAsync("VFX_Explosion_"+ color,
            new Vector3(_emblem.Position.x, _emblem.Position.y, 0f),
            Quaternion.identity,
            board.transform);

            board.RemoveEmblemView(_emblem);

            while (!handler.IsDone)
            {
                yield return null;
            }

            yield return _emblem.DestroyTile();
        }
    }
}

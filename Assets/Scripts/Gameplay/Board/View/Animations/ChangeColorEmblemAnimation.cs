using Board.Model;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Board.View
{
    public class ChangeColorEmblemAnimation : IViewAnimation
    {
        private Vector2Int _position;
        private EmblemItem _item;

        public ChangeColorEmblemAnimation(Vector2Int position, EmblemItem item)
        {
            _position = position;
            _item = item;
        }

        public Coroutine PlayAnimation(BoardView board)
        {
            return board.StartCoroutine(AnimationCoroutine(board, _item));
        }

        private IEnumerator AnimationCoroutine(BoardView board, EmblemItem item)
        {
            AsyncOperationHandle<Sprite> handler = Addressables.LoadAssetAsync<Sprite>($"Sprite_{item.EmblemColor.ToString()}");

            while (!handler.IsDone)
            {
                yield return null;
            }

            board.GetEmblemViewAtPosition(_position).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = handler.Result;
        }
    }
}

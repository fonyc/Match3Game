using Board.Model;
using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Board.View
{
    public class CreateEmblemAnimation : IViewAnimation
    {
        private Vector2Int _position;
        private EmblemItem _item;

        public CreateEmblemAnimation(Vector2Int position, EmblemItem item)
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
            AsyncOperationHandle<GameObject> handler =
            Addressables.InstantiateAsync(item.EmblemColor.ToString(),
            new Vector3(_position.x, board.visualPieceFallPosition, 0f),
            Quaternion.identity,
            board.transform);

            while (!handler.IsDone)
            {
                yield return null;
            }
            EmblemView emblemView = handler.Result.GetComponent<EmblemView>();
            board.AddEmblemView(emblemView);

            yield return emblemView.MoveTo(_position);
        }
    }
}

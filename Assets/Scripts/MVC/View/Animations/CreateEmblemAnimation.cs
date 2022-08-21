using MVC.Model;
using System.Collections;
using UnityEngine;

namespace MVC.View
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
            return board.StartCoroutine(AnimationCoroutine(board));
        }

        private IEnumerator AnimationCoroutine(BoardView board)
        {
            GameObject emblem = GameObject.Instantiate(
                board.EmblemPrefabs[(int)_item.EmblemColor],
                new Vector3(_position.x, board.visualPieceFallPosition, 0),
                Quaternion.identity,
                board.transform);

            EmblemView emblemView = emblem.GetComponent<EmblemView>();

            board.AddEmblemView(emblemView);
            yield return emblemView.MoveTo(_position);
        }
    }
}

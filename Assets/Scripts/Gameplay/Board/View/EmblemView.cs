using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace Board.View
{
    public class EmblemView : MonoBehaviour
    {
        public Vector2Int Position;

        public EmblemView(int xPosition, int yPosition)
        {
            Position = new Vector2Int(xPosition, yPosition);
        }

        public virtual Coroutine MoveTo(Vector2Int newPosition)
        {
            return StartCoroutine(MoveTo_Coro(newPosition));
        }

        private IEnumerator MoveTo_Coro(Vector2Int newPosition)
        {
            Position = newPosition;
            transform.DOMove(new Vector3(newPosition.x, newPosition.y), 0.15f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(0.15f);
        }

        public virtual Coroutine DestroyTile()
        {
            return StartCoroutine(DestroyTile_Coro());
        }

        private IEnumerator DestroyTile_Coro()
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(0f, 0.1f).SetEase(Ease.InOutBounce);
            yield return new WaitForSeconds(0.15f);
            
            Destroy(gameObject);
        }
    }
}

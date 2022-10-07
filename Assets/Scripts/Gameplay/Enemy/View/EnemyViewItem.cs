using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class EnemyViewItem : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;

    private event Action _onAnimationFinished;

    public void Initialize(Action OnAnimationFinished)
    {
        _onAnimationFinished = OnAnimationFinished;
        startPosition = transform.position;
        endPosition = new Vector3(startPosition.x, startPosition.y - 1f, startPosition.z);
    }

    public void StartAttackAnimation()
    {
        StartCoroutine(AttackAnimation());
    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        transform.DOMove(new Vector3(endPosition.x, endPosition.y), 0.2f).SetEase(Ease.InOutQuad).OnComplete(AnimationFinished);
        yield return new WaitForSeconds(0.3f);
        transform.DOMove(new Vector3(startPosition.x, startPosition.y), 0.2f).SetEase(Ease.InOutQuad);
    }

    private void AnimationFinished()
    {
        _onAnimationFinished?.Invoke();
    }
}

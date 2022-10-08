using DG.Tweening;
using System;
using UnityEngine.UI;

public class PlayerAnimations
{
    private event Action _onAnimationFinished;

    public void Initialize(Action OnAnimationFinished)
    {
        _onAnimationFinished = OnAnimationFinished;
    }

    public void DamageAnimation(Image imageColor)
    {
        imageColor.DOFade(0.5f, .5f).OnComplete(() => Restore(imageColor));
    }

    private void Restore(Image imageColor)
    {
        imageColor.DOFade(0f, .5f);
    }
}

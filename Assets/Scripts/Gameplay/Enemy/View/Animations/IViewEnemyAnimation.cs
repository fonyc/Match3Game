using Board.View;
using System.Collections;
using UnityEngine;


public interface IViewEnemyAnimation
{
    Coroutine PlayAnimation(EnemyView enemy);
}

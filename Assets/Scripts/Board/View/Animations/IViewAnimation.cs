using UnityEngine;

namespace Board.View
{
    public interface IViewAnimation
    {
        Coroutine PlayAnimation(BoardView board);
    }
}
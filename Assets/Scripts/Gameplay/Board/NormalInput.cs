using Board.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class NormalInput : BoardInput
{
    public override string Id => base.Id;

    public override void ProcessInput(BoardController controller)
    {
        
    }
}

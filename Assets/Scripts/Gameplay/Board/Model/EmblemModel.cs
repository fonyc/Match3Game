using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Board.Model
{
    public class EmblemModel
    {
        public Vector2Int Position;
        public EmblemItem Item;
        public bool IsEmpty() => Item == null;
    }
}

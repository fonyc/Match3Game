using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : IConsumable
{
    public void Consume()
    {
        Debug.Log("Que rica pocion de mana");
    }
}

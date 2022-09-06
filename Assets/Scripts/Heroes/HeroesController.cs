using Shop.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesController 
{
    public HeroModel Model { get; private set; }

    public UserData UserData { get; private set; }

    public HeroesController(UserData userData)
    {
        UserData = userData;
    }
}

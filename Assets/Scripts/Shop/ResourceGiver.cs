using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGiver 
{
    private UserData _userData;

    public ResourceGiver(UserData data)
    {
        _userData = data;
    }

    public void AddGold()
    {
        _userData.AddGold();
    }
}

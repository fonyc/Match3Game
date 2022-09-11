using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController 
{
    public HeroModel heroModel;
    //public ItemModel battleItemModel;

    public TeamController(UserData userData)
    {

    }

    public void Initialize()
    {
        Load();
    }

    private void Load()
    {
        heroModel = JsonUtility.FromJson<HeroModel>(Resources.Load<TextAsset>("HeroModel").text);
    }
}

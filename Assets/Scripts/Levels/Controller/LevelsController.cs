using Shop.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsController 
{
    public LevelModel LevelModel;
    public EnemyModel EnemyModel;
    private UserData _userData;

    public LevelsController(UserData userData)
    {
        _userData = userData;
    }

    public void Initialize()
    {
        Load();
    }

    public void Load()
    {
        LevelModel = JsonUtility.FromJson<LevelModel>(Resources.Load<TextAsset>("LevelsModel").text);
    }
}

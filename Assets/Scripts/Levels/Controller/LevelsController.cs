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

    public void ChangeScene(int level)
    {
        Debug.Log("Changing scene to level " + level);
    }

    public void Load()
    {
        LevelModel = JsonUtility.FromJson<LevelModel>(Resources.Load<TextAsset>("LevelsModel").text);
    }
}

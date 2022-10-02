using UnityEngine;

public class LevelsController 
{
    public LevelModel LevelModel;
    public EnemiesModel EnemyModel;
    private UserData _userData;
    private SceneLoader _sceneLoader;

    public LevelsController(UserData userData, SceneLoader sceneLoader)
    {
        _userData = userData;
        _sceneLoader = sceneLoader;
    }

    public void Initialize()
    {
        Load();
    }

    public void ChangeGameplayScene(int level)
    {
        _userData.SetCurrentSelectedLevel(level);
        _userData.Save();

        _sceneLoader.ChangeScene(2);
    }

    public void Load()
    {
        LevelModel = JsonUtility.FromJson<LevelModel>(Resources.Load<TextAsset>("LevelsModel").text);
    }
}

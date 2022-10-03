using Shop.Model;
using UnityEngine;

public class HeroesController 
{
    public HeroModel Model { get; private set; }

    public UserData UserData { get; private set; }

    public HeroesController(UserData userData)
    {
        UserData = userData;
    }

    public void Initialize()
    {
        Load();
    }

    private void Load()
    {
        //Model = JsonUtility.FromJson<HeroModel>(Resources.Load<TextAsset>("HeroModel").text);
        Model = new HeroModel();
        Model.Heroes = ServiceLocator.GetService<GameConfigService>().HeroModel;
    }
}

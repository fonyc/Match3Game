using System;
using UnityEngine;

public class PlayerController: IDisposable
{
    private UserData _userData;
    private PlayerModel _playerModel;
    private ItemController _itemController;

    public event Action<int> OnHPChanged = delegate (int amount) { };
    public event Action<int> OnATKChanged = delegate (int amount) { };
    public event Action<int> OnDEFChanged = delegate (int amount) { };

    public PlayerController(UserData userData, ItemController itemController)
    {
        _itemController = itemController;
        _itemController.OnHPChanged += ChangeHP;
        _itemController.OnATKChanged += ChangeATK;
        _itemController.OnDEFChanged += ChangeDEF;

        _userData = userData;
        _playerModel = new PlayerModel();
    }

    public void Initialize()
    {
        LoadSelectedHero();
    }

    public HeroItemModel GetHero()
    {
        return _playerModel.hero;
    }

    public HeroStats GetCurrentStats()
    {
        return _playerModel.currentHeroStats;
    }

    private void LoadSelectedHero()
    {
        HeroModel allHeroesModel = JsonUtility.FromJson<HeroModel>(Resources.Load<TextAsset>("HeroModel").text);
        _playerModel.hero = GetHeroData(allHeroesModel);
        _playerModel.currentHeroStats = _playerModel.hero.Stats;
    }

    private HeroItemModel GetHeroData(HeroModel allHeroesModel)
    {
        foreach (HeroItemModel hero in allHeroesModel.Heroes)
        {
            if (hero.Id == _userData.GetSelectedHero()) return hero;
        }
        return null;
    }

    #region EVENTS
    public void ChangeHP(int amount)
    {
        int maxHP = _playerModel.hero.Stats.HP;
        int currentHeroHP = _playerModel.currentHeroStats.HP;
        _playerModel.currentHeroStats.HP = Mathf.Clamp(currentHeroHP + amount, 0, maxHP);
        OnHPChanged.Invoke(_playerModel.currentHeroStats.HP);
        //if(CheckPlayerDeath())
    }

    public void ChangeATK(int amount)
    {
        _playerModel.currentHeroStats.ATK += amount;
        OnATKChanged.Invoke(_playerModel.currentHeroStats.ATK);
    }

    public void ChangeDEF(int amount)
    {
        _playerModel.currentHeroStats.DEF += amount;
        OnDEFChanged.Invoke(_playerModel.currentHeroStats.DEF);
    }
    #endregion

    private bool CheckPlayerDeath()
    {
        return _playerModel.currentHeroStats.HP == 0;
    }

    public void Dispose()
    {
        _itemController.OnHPChanged -= ChangeHP;
        _itemController.OnATKChanged -= ChangeATK;
        _itemController.OnDEFChanged -= ChangeDEF;
    }
}
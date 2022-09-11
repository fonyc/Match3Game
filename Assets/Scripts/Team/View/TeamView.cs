using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamView : MonoBehaviour
{
    [SerializeField]
    private TeamHeroView _heroSelectPrefab = null;

    [SerializeField]
    private Transform _heroesParent = null;

    [SerializeField]
    private TeamBattleItemView _battleItemSelectPrefab = null;

    [SerializeField]
    private Transform _battleItemsParent = null;

    private TeamController _teamController;

    private UserData _userData;

    public void Initialize(TeamController teamController, UserData userData)
    {
        _teamController = teamController;
        _userData = userData;
        userData.OnHeroAdded += CreateHeroCollection;
        userData.OnBattleItemAdded += CreateBattleItemCollection;

        CreateHeroCollection();
        CreateBattleItemCollection();
    }

    private void CreateHeroCollection()
    {
        while (_heroesParent.childCount > 0)
        {
            Transform child = _heroesParent.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        foreach (OwnedHero ownedHero in _userData.GetOwnedHeroes())
        {
            foreach(HeroItemModel heroModel in _teamController.heroModel.Heroes)
            {
                if (ownedHero.Name != heroModel.Name) continue;
                Instantiate(_heroSelectPrefab, _heroesParent).SetData(heroModel, _userData);
            }
        }
    }

    private void CreateBattleItemCollection()
    {
        while (_battleItemsParent.childCount > 0)
        {
            Transform child = _battleItemsParent.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        foreach (OwnedBattleItem item in _userData.GetOwnedBattleItems())
        {
            Instantiate(_battleItemSelectPrefab, _battleItemsParent);//.SetData(heroModel, _userData);
        }
    }
}
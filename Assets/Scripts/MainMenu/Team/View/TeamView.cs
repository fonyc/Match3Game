using DG.Tweening;
using Shop.Controller;
using Shop.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamView : MonoBehaviour, IMainMenuAnimation
{
    #region Variables
    [Header("--- HERO SELECTION ---")]
    [Space(5)]
    [SerializeField]
    private TeamHeroView _heroSelectPrefab = null;

    [SerializeField]
    private Transform _heroesParent = null;

    [Header("--- ITEM SELECTION ---")]
    [Space(5)]
    [SerializeField]
    private TeamBattleItemView _battleItemSelectPrefab = null;

    [SerializeField]
    private Transform _battleItemsParent = null;

    private TeamController _teamController;

    private UserData _userData;

    private List<TeamHeroView> heroesToSelect = new();
    private List<TeamBattleItemView> itemsToSelect = new();

    #endregion

    public string Id { get => "Team"; set { } }

    public void AppearAnimation(RectTransform rect, float delay)
    {
        gameObject.SetActive(true);
        StartCoroutine(AppearAnimation_Coro(rect, delay));
    }

    public IEnumerator AppearAnimation_Coro(RectTransform rect, float delay)
    {
        yield return new WaitForSeconds(delay);
        rect.DOAnchorPos(Vector2.zero, 0.3f).SetEase(Ease.OutBack);
    }

    public void HideAnimation(RectTransform rect)
    {
        rect.DOAnchorPos(new Vector2(0, -2500f), 0.25f).SetEase(Ease.InBack).OnComplete(Hide);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Initialize(TeamController teamController, UserData userData)
    {
        _teamController = teamController;
        _userData = userData;
        _userData.OnHeroAdded += CreateHeroCollection;
        _userData.OnBattleItemAdded += CreateBattleItemCollection;
        _userData.OnBattleItemModified += CreateBattleItemCollection;

        CreateHeroCollection();
        OnHeroSelected(_userData.GetSelectedHero());

        CreateBattleItemCollection();
        UpdateSelectedItemsOnStart();
    }

    private void OnHeroSelected(string heroName)
    {
        if (heroName == null) return;
        _teamController.SelectHero(heroName);

        foreach(TeamHeroView hero in heroesToSelect)
        {
            hero.SetTick(hero.GetId() == heroName);
        }
    }

    private void OnItemSelected(string itemName)
    {
        if (itemName == null) return;
        _teamController.SelectItem(itemName);

        foreach (TeamBattleItemView battleItem in itemsToSelect)
        {
            battleItem.SetTick(false);
        }

        foreach (TeamBattleItemView battleItem in itemsToSelect)
        {
            foreach (string item in _userData.GetSelectedItems())
            {
                if (battleItem.GetId() == item) battleItem.SetTick(true);
            }
        }
        
    }

    private void UpdateSelectedItemsOnStart()
    {
        foreach (TeamBattleItemView battleItem in itemsToSelect)
        {
            foreach (string item in _userData.GetSelectedItems())
            {
                if (battleItem.GetId() == item) battleItem.SetTick(true);
            }
        }
    }

    private void CreateHeroCollection()
    {
        heroesToSelect.Clear();

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
                if (ownedHero.Id != heroModel.Id) continue;
                TeamHeroView item = Instantiate(_heroSelectPrefab, _heroesParent);
                item.SetData(heroModel, OnHeroSelected);
                heroesToSelect.Add(item);
            }
        }
        OnHeroSelected(_userData.GetSelectedHero());
    }

    private void CreateBattleItemCollection()
    {
        itemsToSelect.Clear();
        while (_battleItemsParent.childCount > 0)
        {
            Transform child = _battleItemsParent.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        foreach (OwnedBattleItem item in _userData.GetOwnedBattleItems())
        {
            foreach(BattleItemModel battleItemModel in _teamController.battleItemModel.BattleItems)
            {
                if (battleItemModel.Id != item.Id) continue;
                TeamBattleItemView createdItem = Instantiate(_battleItemSelectPrefab, _battleItemsParent);
                createdItem.SetData(battleItemModel, _userData, OnItemSelected);
                itemsToSelect.Add(createdItem);
            }
        }
        UpdateSelectedItemsOnStart();
    }
}
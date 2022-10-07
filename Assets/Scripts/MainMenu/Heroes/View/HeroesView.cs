using DG.Tweening;
using System.Collections;
using UnityEngine;

public class HeroesView : MonoBehaviour, IMainMenuAnimation
{
    [SerializeField]
    private HeroItemView _heroItemPrefab = null;

    [SerializeField]
    private Transform _itemsParent = null;

    private HeroesController _controller;

    private UserData _userData;

    public string Id { get => "Heroes"; set { } }

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
        rect.DOAnchorPos(new Vector2(0, 2500f), 0.25f).SetEase(Ease.InBack).OnComplete(Hide);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Initialize(HeroesController controller, UserData userData)
    {
        _userData = userData;
        _controller = controller;
        _userData.OnHeroAdded += CreateHeroCollection;

        CreateHeroCollection();
    }

    private void CreateHeroCollection()
    {
        while (_itemsParent.childCount > 0)
        {
            Transform child = _itemsParent.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        foreach (OwnedHero ownedHero in _userData.GetOwnedHeroes())
        {
            foreach (HeroItemModel heroModel in _controller.Model.Heroes)
            {
                if (ownedHero.Id != heroModel.AvatarImage) continue;
                Instantiate(_heroItemPrefab, _itemsParent).SetData(heroModel, _userData);
            }
        }
    }

    private void OnDestroy()
    {
        _userData.OnHeroAdded -= CreateHeroCollection;
    }
}

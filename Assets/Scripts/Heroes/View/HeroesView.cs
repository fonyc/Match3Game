using UnityEngine;

public class HeroesView : MonoBehaviour
{
    [SerializeField]
    private HeroItemView _heroItemPrefab = null;

    [SerializeField]
    private Transform _itemsParent = null;

    private HeroesController _controller;

    private UserData _userData;

    public void Initialize(HeroesController controller, UserData userData)
    {
        _userData = userData;
        _controller = controller;
        userData.OnHeroAdded += CreateHeroCollection;

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
            foreach(HeroItemModel heroModel in _controller.Model.Heroes)
            {
                if (ownedHero.Name != heroModel.AvatarImage) continue;
                Instantiate(_heroItemPrefab, _itemsParent).SetData(heroModel, _userData);
            }
        }
    }
}

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
        //Ensure there are no previous items
        while (_itemsParent.childCount > 0)
        {
            Transform child = _itemsParent.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        //Instantiate the owned heroes 
        foreach (OwnedHero ownedHero in _userData.GetOwnedHeroList())
        {
            foreach(HeroItemModel heroModel in _controller.Model.Heroes)
            {
                if (ownedHero.Name != heroModel.AvatarImage) continue;
                Instantiate(_heroItemPrefab, _itemsParent).SetData(heroModel, _userData);
            }
        }
    }
}

using UnityEngine;

public class LevelsView : MonoBehaviour
{
    [SerializeField]
    private LevelItemView _levelItemPrefab = null;

    [SerializeField]
    private Transform _itemsParent = null;

    LevelsController _levelsController = null;

    public void Initialize(LevelsController levelsController, UserData userData)
    {
        _levelsController = levelsController;

        while (_itemsParent.childCount > 0)
        {
            Transform child = _itemsParent.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        foreach (LevelModelItem levelModelItem in _levelsController.LevelModel.Levels)
        {
            LevelItemView item = Instantiate(_levelItemPrefab, _itemsParent);
            item.SetData(levelModelItem, userData, OnLevelSelected);
            item.Initialize();
        }
    }

    public void OnLevelSelected(int levelSelected)
    {
        _levelsController.ChangeGameplayScene(levelSelected);
    }
}

using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LevelsView : MonoBehaviour, IMainMenuAnimation
{
    [SerializeField]
    private LevelItemView _levelItemPrefab = null;

    [SerializeField]
    private Transform _itemsParent = null;

    LevelsController _levelsController = null;

    public string Id { get => "Levels"; set { } }

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
        rect.DOAnchorPos(new Vector2(-2500f, 0), 0.25f).SetEase(Ease.InBack).OnComplete(Hide);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

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

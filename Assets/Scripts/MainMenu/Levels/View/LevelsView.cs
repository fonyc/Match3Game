using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LevelsView : MonoBehaviour, IMainMenuAnimation
{
    [SerializeField]
    private LevelItemView _levelItemPrefab = null;

    [SerializeField]
    private Transform _itemsParent = null;

    [SerializeField]
    private GameObject _noHeroPanel = null;
    private GameObject heroPanel = null;

    LevelsController _levelsController = null;

    GameProgressionService _gameProgression;

    #region MAIN MENU ANIMATIONS
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
    #endregion

    public void Initialize(LevelsController levelsController, GameProgressionService gameProgression)
    {
        _gameProgression = gameProgression;
        _gameProgression.OnHeroSelected += RemoveHeroPanel;
        if (string.IsNullOrEmpty(gameProgression.GetSelectedHero())) heroPanel = Instantiate(_noHeroPanel, transform);

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
            item.SetData(levelModelItem, gameProgression, OnLevelSelected);
            item.Initialize();
        }
    }

    private void RemoveHeroPanel()
    {
        Destroy(heroPanel);
    }

    public void OnLevelSelected(int levelSelected)
    {
        _levelsController.ChangeGameplayScene(levelSelected);
    }

    private void OnDestroy()
    {
        _gameProgression.OnHeroSelected -= RemoveHeroPanel;
    }
}

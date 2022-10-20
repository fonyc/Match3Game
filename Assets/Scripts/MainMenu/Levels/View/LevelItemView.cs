using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class LevelItemView : MonoBehaviour
{
    [SerializeField]
    private Transform parent;

    [SerializeField]
    private LevelRewardView _levelRewardPrefab;

    [SerializeField]
    private Image _dragonImage = null;

    [SerializeField]
    private TMP_Text _enemyName = null;

    [SerializeField]
    private TMP_Text _levelNumber = null;

    LevelModelItem _levelItemModel;

    private GameProgressionService _gameProgression;

    event Action<int> _onClickedEvent;

    public void SetData(LevelModelItem levelItemModel, GameProgressionService gameProgression, Action<int> onClickedEvent)
    {
        _gameProgression = gameProgression;
        _levelItemModel = levelItemModel;
        _onClickedEvent = onClickedEvent;
        SetVisuals();
    }

    public void Initialize()
    {
        while (parent.childCount > 0)
        {
            Transform child = parent.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        foreach (ResourceItem levelModelItem in _levelItemModel.Rewards)
        {
            Instantiate(_levelRewardPrefab, parent).SetData(levelModelItem);
        }
    }

    public void OnClickedButton()
    {
        _onClickedEvent?.Invoke(_levelItemModel.Level);
    }

    private void SetVisuals()
    {
        GetComponent<Button>().interactable = IsInteractable();

        Addressables.LoadAssetAsync<Sprite>(_levelItemModel.Enemy).Completed += handler =>
        {
            _dragonImage.sprite = handler.Result;
        };

        _enemyName.text = _levelItemModel.Enemy;
        _levelNumber.text = "LEVEL " + (_levelItemModel.Level + 1).ToString();
    }

    private bool IsInteractable()
    {
        return _gameProgression.GetLevelsPassed() >= _levelItemModel.Level;
    }
}

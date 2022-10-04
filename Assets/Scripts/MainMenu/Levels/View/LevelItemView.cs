using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelItemView : MonoBehaviour
{
    [SerializeField]
    private Transform parent;

    [SerializeField]
    private LevelRewardView _levelRewardPrefab;

    [SerializeField]
    private List<Sprite> _dragonSprites = new List<Sprite>();

    [SerializeField]
    private Image _dragonImage = null;

    [SerializeField]
    private TMP_Text _enemyName = null;

    [SerializeField]
    private TMP_Text _levelNumber = null;

    LevelModelItem _levelItemModel;

    private UserData _userData;

    event Action<int> _onClickedEvent;

    public void SetData(LevelModelItem levelItemModel, UserData userData, Action<int> onClickedEvent)
    {
        _userData = userData;
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
        _dragonImage.sprite = _dragonSprites.Find(sprite => sprite.name == _levelItemModel.Enemy);
        _enemyName.text = _levelItemModel.Enemy;
        _levelNumber.text = "LEVEL " + _levelItemModel.Level.ToString();
    }

    private bool IsInteractable()
    {
        return _userData.GetLevelsPassed() + 1 >= _levelItemModel.Level;
    }
}

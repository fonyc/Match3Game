using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelItemView : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> _dragonSprites = new List<Sprite>();

    [SerializeField]
    private Image _dragonImage = null;

    [SerializeField]
    private TMP_Text _enemyName = null;

    [SerializeField]
    private TMP_Text _levelNumber = null;

    [SerializeField]
    private TMP_Text _gemsRewardAmount = null;

    [SerializeField]
    private TMP_Text _goldRewardAmount = null;

    LevelModelItem _levelItemModel;
    UserData _userData;

    event Action<int> _onClickedEvent;

    public void SetData(LevelModelItem levelItemModel, UserData userData, Action<int> onClickedEvent)
    {
        _levelItemModel = levelItemModel;
        _userData = userData;
        _onClickedEvent = onClickedEvent;
        SetVisuals();
    }

    public void OnClickedButton(int level)
    {
        _onClickedEvent?.Invoke(level);
    }

    private void SetVisuals()
    {
        _dragonImage.sprite = _dragonSprites.Find(sprite => sprite.name == _levelItemModel.Enemy);
        _enemyName.text = _levelItemModel.Enemy;
        _levelNumber.text = "LEVEL " + _levelItemModel.Level.ToString();
        _gemsRewardAmount.text = _levelItemModel.GemsReward.Amount.ToString();
        _goldRewardAmount.text = _levelItemModel.GoldReward.Amount.ToString();
    }
}

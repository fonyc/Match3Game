using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelRewardView : MonoBehaviour
{
    [SerializeField] 
    private List<Sprite> _rewardSprites;
    
    [SerializeField]
    private TMP_Text _qtyReward = null;

    [SerializeField]
    private Image _rewardImg = null;

    private ResourceItem model;

    public void SetData(ResourceItem resourceItem)
    {
        model = resourceItem;
        SetVisuals();
    }

    private void SetVisuals()
    {
        _qtyReward.text = model.Amount.ToString();
        _rewardImg.sprite = _rewardSprites.Find(sprite => sprite.name == model.Name);
    }
}

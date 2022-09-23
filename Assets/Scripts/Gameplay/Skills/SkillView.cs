using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    [Header(" --- UI ---")]
    [Space(5)]
    [SerializeField] private Image _skillIcon;
    [SerializeField] private Image _manaFill;
    [SerializeField] private TMP_Text _manaText = null;
    [SerializeField] private List<Sprite> SkillSpriteList = new();
    SkillModel _model;
    UserData _userData;

    public void Initialize(UserData userData)
    {
        _userData = userData;
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        //_skillIcon.sprite = SkillSpriteList.Find(sprite => sprite.name == );
        //_hpFill.fillAmount = _controller.GetCurrentStats().HP * 100 / _controller.GetHero().Stats.HP;
        //_hpText.text = _controller.GetCurrentStats().HP.ToString() + " / " + _controller.GetHero().Stats.HP;
    }

    public void OnClickButton()
    {
        Debug.Log("Skill was pressed");
    }
}

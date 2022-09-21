using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroItemView : MonoBehaviour
{
    #region UI FIELDS
    [SerializeField]
    private List<Sprite> _colorSprites = new List<Sprite>();

    [SerializeField]
    private List<Sprite> _avatarSprites = new List<Sprite>();

    [SerializeField]
    private Image _heroAvatar = null;

    [SerializeField]
    private Image _color = null;

    [SerializeField]
    private TMP_Text _heroName = null;

    [SerializeField]
    private TMP_Text _ATK = null;

    [SerializeField]
    private TMP_Text _DEF = null;

    [SerializeField]
    private TMP_Text _HP = null;

    [SerializeField]
    private TMP_Text _Skill = null;
    #endregion

    private HeroItemModel _model;
    private UserData _userData;

    public void SetData(HeroItemModel model, UserData userData)
    {
        _model = model;
        _userData = userData;
        _userData.OnHeroModified += HeroUpdated;
        UpdateVisuals();
    }

    private void OnDestroy()
    {
        if (_userData != null) _userData.OnHeroModified -= HeroUpdated;
    }

    private void HeroUpdated(string resource)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (_model == null) return;
        _heroAvatar.sprite = _avatarSprites.Find(sprite => sprite.name == _model.AvatarImage);
        _color.sprite = _colorSprites.Find(sprite => sprite.name == _model.ColorImage);
        _heroName.text = _model.Name;
        _ATK.text = ": " + _model.Stats.ATK.ToString();
        _DEF.text = ": " + _model.Stats.DEF.ToString();
        _HP.text = ": " + _model.Stats.HP.ToString();
        _Skill.text = _model.Skill;
    }
}

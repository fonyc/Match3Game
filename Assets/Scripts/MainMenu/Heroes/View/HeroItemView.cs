using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class HeroItemView : MonoBehaviour
{
    #region UI FIELDS
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
    private GameProgressionService _gameProgression;

    public void SetData(HeroItemModel model, GameProgressionService gameProgression)
    {
        _model = model;
        _gameProgression = gameProgression;
        _gameProgression.OnHeroModified += HeroUpdated;
        UpdateVisuals();
    }

    private void OnDestroy()
    {
        if (_gameProgression != null) _gameProgression.OnHeroModified -= HeroUpdated;
    }

    private void HeroUpdated(string resource)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (_model == null) return;

        Addressables.LoadAssetAsync<Sprite>(_model.AvatarImage).Completed += handler =>
        {
            _heroAvatar.sprite = handler.Result;
        };

        Addressables.LoadAssetAsync<Sprite>($"Sprite_{_model.ColorImage}").Completed += handler =>
        {
            _color.sprite = handler.Result;
        };

        _heroName.text = _model.Name;
        _ATK.text = ": " + _model.Stats.ATK.ToString();
        _DEF.text = ": " + _model.Stats.DEF.ToString();
        _HP.text = ": " + _model.Stats.HP.ToString();
        _Skill.text = _model.Skill;
    }
}

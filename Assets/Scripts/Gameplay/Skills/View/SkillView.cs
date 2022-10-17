using DG.Tweening;
using JetBrains.Annotations;
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

    private SkillController _controller;
    private StatTripleIntArgument_Event _onPlayerAttacks;

    public void Initialize(SkillController skillController, StatTripleIntArgument_Event OnPlayerAttacks)
    {
        _controller = skillController;
        _onPlayerAttacks = OnPlayerAttacks;
        _onPlayerAttacks.AddListener(GenerateMana);
        _controller.OnManaChanged += UpdateMana;

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        _skillIcon.sprite = SkillSpriteList.Find(sprite => sprite.name == _controller.GetSkillItemModel().Id);
        _manaFill.fillAmount = _controller.GetCurrentPlayerMana() * 100 / _controller.GetSkillItemModel().Mana;
        _manaText.text = _controller.GetCurrentPlayerMana().ToString() + " / " + _controller.GetSkillItemModel().Mana;
    }

    private void GenerateMana(Stats stats, int hits, int color, int columns)
    {
        _controller.AddMana(hits + columns, stats.ManaPerHit);
    }

    private void UpdateMana(int mana, int maxMana)
    {
        _manaFill.DOFillAmount(SetFillAmount(mana, maxMana), 0.5f);
        _manaText.text = mana.ToString() + " / " + maxMana.ToString();
    }

    public void OnClickButton()
    {
        _controller.PerformSkill();
    }

    private float SetFillAmount(int amount, int max)
    {
        float qty = (float)amount / (float)max;
        if (qty <= 0f) return 0f;
        else return qty;
    }

    private void OnDestroy()
    {
        _onPlayerAttacks.RemoveListener(GenerateMana);
    }
}

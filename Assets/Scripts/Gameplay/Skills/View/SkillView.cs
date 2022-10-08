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
    private StatIntIntArgument_Event _onPlayerAttacks;

    public void Initialize(SkillController skillController, StatIntIntArgument_Event OnPlayerAttacks)
    {
        _onPlayerAttacks = OnPlayerAttacks;
        _onPlayerAttacks.AddListener(GenerateMana);

        _controller = skillController;
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        _skillIcon.sprite = SkillSpriteList.Find(sprite => sprite.name == _controller.GetSkillItemModel().Id);
        _manaFill.fillAmount = _controller.GetCurrentPlayerMana() * 100 / _controller.GetSkillItemModel().Mana;
        _manaText.text = _controller.GetCurrentPlayerMana().ToString() + " / " + _controller.GetSkillItemModel().Mana;
    }

    private void GenerateMana(Stats stats, int hits, int color)
    {
        _controller.AddMana(hits);
    }

    public void OnClickButton()
    {
        _controller.PerformSkill();
    }

    private void OnDestroy()
    {
        _onPlayerAttacks.RemoveListener(GenerateMana);
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [Header(" --- PLAYER ---")]
    [Space(5)]
    [SerializeField] private Image _heroImage;
    [SerializeField] private TMP_Text _hpText = null;
    [SerializeField] private Image _hpFill;
    [Header(" --- ITEMS ---")]
    [Space(5)]
    [SerializeField] private Image _item1;
    [SerializeField] private Image _item2;
    [SerializeField] private TMP_Text _item1Qty = null;
    [SerializeField] private TMP_Text _item2Qty = null;
    [Header(" --- SKILL ---")]
    [Space(5)]
    [SerializeField] private Image skill;
    [SerializeField] private Image _manaFill;
    [SerializeField] private TMP_Text _manaText = null;
    [Header(" --- SPRITES ---")]
    [Space(5)]
    [SerializeField] List<Sprite> ItemSpriteList = new();
    [SerializeField] List<Sprite> SkillSpriteList = new();
    [SerializeField] List<Sprite> HeroSpriteList = new();

    private UserData _userData;
    private PlayerController _controller;

    public void Initialize(PlayerController controller, UserData userData)
    {
        _controller = controller;
        _userData = userData;
        UpdateHealthVisuals();
    }

    public void UpdateHealthVisuals()
    {
        _heroImage.sprite = HeroSpriteList.Find(sprite => sprite.name == _controller.GetHero().AvatarImage);
        _hpFill.fillAmount = _controller.GetCurrentStats().HP * 100 / _controller.GetHero().Stats.HP;
        _hpText.text = _controller.GetCurrentStats().HP.ToString();
    }

    public void UpdateItemVisuals()
    {
        _item1.sprite = ItemSpriteList.Find(sprite => sprite.name == _controller.GetItem(0).Id);
        _item2.sprite = ItemSpriteList.Find(sprite => sprite.name == _controller.GetItem(1).Id);
        _item1Qty.text = _controller.GetHero().MaxItems.ToString();
        _item2Qty.text = _controller.GetHero().MaxItems.ToString();
    }
}

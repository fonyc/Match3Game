using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [Header(" --- UI ---")]
    [Space(5)]
    [SerializeField] private Image _heroImage;
    [SerializeField] private TMP_Text _hpText = null;
    [SerializeField] private Image _hpFill;
    [Header(" --- SPRITES ---")]
    [Space(5)]
    [SerializeField] List<Sprite> HeroSpriteList = new();

    //[Header(" --- SKILL ---")]
    //[Space(5)]
    //[SerializeField] private Image skill;
    //[SerializeField] private Image _manaFill;
    //[SerializeField] private TMP_Text _manaText = null;

    private UserData _userData;
    private PlayerController _controller;

    public void Initialize(PlayerController controller, UserData userData)
    {
        _controller = controller;
        _userData = userData;
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        _heroImage.sprite = HeroSpriteList.Find(sprite => sprite.name == _controller.GetHero().AvatarImage);
        _hpFill.fillAmount = _controller.GetCurrentStats().HP * 100 / _controller.GetHero().Stats.HP;
        _hpText.text = _controller.GetCurrentStats().HP.ToString() + " / " + _controller.GetHero().Stats.HP;
    }

}

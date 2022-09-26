using System;
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
    [Header(" --- BUFFS ---")]
    [Space(5)]
    [SerializeField] private GameObject ATKbuffPrefab;
    [SerializeField] private GameObject DEFbuffPrefab;

    [Header(" --- SPRITES ---")]
    [Space(5)]
    [SerializeField] List<Sprite> HeroSpriteList = new();

    private UserData _userData;
    private PlayerController _controller;

    public void Initialize(PlayerController controller, UserData userData)
    {
        _controller = controller;
        _controller.OnATKChanged += AddATKBuff;
        _controller.OnDEFChanged += AddDEFBuff;
        _controller.OnHPChanged += ChangeHP;
        _userData = userData;
        SetInitialStats();
    }

    public void SetInitialStats()
    {
        _heroImage.sprite = HeroSpriteList.Find(sprite => sprite.name == _controller.GetHero().AvatarImage);
        _hpFill.fillAmount = _controller.GetCurrentStats().HP * 100 / _controller.GetHero().Stats.HP;
        _hpText.text = _controller.GetCurrentStats().HP.ToString() + " / " + _controller.GetHero().Stats.HP;
    }

    private void ChangeHP(int amount)
    {
        _hpFill.fillAmount = amount;
        _hpText.text = amount + " / " + _controller.GetHero().Stats.HP;
    }

    private void AddATKBuff(int amount)
    {
        ATKbuffPrefab.SetActive(true);
    }
    
    private void AddDEFBuff(int amount)
    {
        DEFbuffPrefab.SetActive(true);
    }

    private void OnDestroy()
    {
        _controller.OnATKChanged -= AddATKBuff;
    }
}

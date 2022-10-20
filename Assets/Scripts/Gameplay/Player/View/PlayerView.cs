using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [Header(" --- UI ---")]
    [Space(5)]
    [SerializeField] private Image _heroImage;
    [SerializeField] private TMP_Text _hpText = null;
    [SerializeField] private Image _hpFill;
    [SerializeField] private Image _damageColor;

    [Header(" --- BUFFS ---")]
    [Space(5)]
    [SerializeField] private GameObject ATKbuffPrefab;
    [SerializeField] private GameObject DEFbuffPrefab;

    private PlayerAnimations _playerAnimations;

    private PlayerController _controller;
    private TripleIntArgument_Event _onEmblemsDestroyed;
    private StatIntIntArgument_Event _onEnemyAttacks;
    private NoArgument_Event _onBuffsReset;

    public void Initialize(PlayerController controller, TripleIntArgument_Event OnEmblemsDestroyed, 
        StatIntIntArgument_Event OnEnemyAttacks, NoArgument_Event OnBuffsReset)
    {
        _onBuffsReset = OnBuffsReset;
        _onEnemyAttacks = OnEnemyAttacks;
        _onEmblemsDestroyed = OnEmblemsDestroyed;
        _controller = controller;

        _onBuffsReset.AddListener(ResetStats);
        _onEnemyAttacks.AddListener(RecieveAttack);
        _onEmblemsDestroyed.AddListener(PrepareAttack);
        _controller.OnATKChanged += AddATKBuff;
        _controller.OnDEFChanged += AddDEFBuff;
        _controller.OnHPChanged += ChangeHP;
        _controller.OnStatsCleaned += ResetBuffVisuals;

        _playerAnimations = new PlayerAnimations();
        SetInitialStats();
    }

    private void PrepareAttack(int hits, int colorAttack, int columns)
    {
        _controller.AttackEnemy(hits, colorAttack, columns);
    }

    private void RecieveAttack(Stats enemyStats, int hits, int color)
    {
        _controller.RecieveAttack(enemyStats, hits, color);
        _playerAnimations.DamageAnimation(_damageColor);
    }

    public void SetInitialStats()
    {
        Addressables.LoadAssetAsync<Sprite>(_controller.GetHero().AvatarImage).Completed += handler =>
        {
            _heroImage.sprite = handler.Result;
        };

        _hpFill.fillAmount = _controller.GetCurrentStats().HP / _controller.GetHero().Stats.HP;
        _hpText.text = _controller.GetCurrentStats().HP.ToString() + " / " + _controller.GetHero().Stats.HP;
    }

    private void ChangeHP(int amount, int max)
    {
        _hpFill.DOFillAmount(SetFillAmount(amount, max), 0.5f);
        _hpText.text = amount + " / " + max;
    }

    private void AddATKBuff()
    {
        ATKbuffPrefab.SetActive(true);
    }
    
    private void AddDEFBuff()
    {
        DEFbuffPrefab.SetActive(true);
    }

    private void ResetStats()
    {
        _controller.CleanStats();
    }

    private void ResetBuffVisuals()
    {
        ATKbuffPrefab.SetActive(false);
        DEFbuffPrefab.SetActive(false);
    }

    private float SetFillAmount(int hp, int max)
    {
        float qty = (float)hp / (float)max;
        if (qty <= 0f) return 0f;
        else return qty;
    }

    private void OnDestroy()
    {
        _onEmblemsDestroyed.RemoveListener(PrepareAttack);
        _controller.OnATKChanged -= AddATKBuff;
        _controller.OnDEFChanged -= AddDEFBuff;
        _controller.OnHPChanged -= ChangeHP;
    }
}

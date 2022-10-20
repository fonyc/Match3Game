using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour
{
    #region UI VARIABLES
    [SerializeField]
    private Image _dragonImage = null;

    [SerializeField]
    private Image _hpFill = null;
    
    [SerializeField]
    private Image _damageColor = null;

    [SerializeField]
    private TMP_Text _dragonAndHPText = null;
    private string _dragonName = "";

    [SerializeField]
    private EnemyViewItem _enemyAnimations;
    #endregion

    EnemyController _controller;

    StatTripleIntArgument_Event _onPlayerAttacked;
    IntArgument_Event _onMovesAvailableChanged;

    public void Initialize(EnemyController enemyController, StatTripleIntArgument_Event OnPlayerAttacked, 
        IntArgument_Event OnMovesAvailableChanged)
    {
        _controller = enemyController;

        _enemyAnimations.Initialize(OnAttackAnimationFinished);

        _onMovesAvailableChanged = OnMovesAvailableChanged;
        _onMovesAvailableChanged.AddListener(OnMovesAreOver);
        _controller.OnHPChanged += ChangeHP;
        _onPlayerAttacked = OnPlayerAttacked;
        _onPlayerAttacked.AddListener(TranslateAttackToDamage);

        SetStartingVisuals();
    }

    private void OnMovesAreOver(int moves)
    {
        if (moves == 0) _enemyAnimations.StartAttackAnimation();
    }

    private void OnAttackAnimationFinished()
    {
        _controller.AttackPlayer();
    }

    private void ChangeHP(int hp, int max)
    {
        _hpFill.DOFillAmount(SetFillAmount(), 0.25f);
        _dragonAndHPText.text = _dragonName.ToUpper() + " " + "(" + hp + " / " + max + ")";
    }

    private void TranslateAttackToDamage(Stats heroStats, int hits, int heroColor, int columns)
    {
        _controller.RecieveDamageFromPlayer(heroStats, hits, heroColor, columns);
        _enemyAnimations.DamageAnimation(_damageColor);
    }

    private void SetStartingVisuals()
    {
        _dragonName = _controller.Model.Enemy.Name;

        Addressables.LoadAssetAsync<Sprite>(_controller.Model.Enemy.Id).Completed += handler =>
        {
            _dragonImage.sprite = handler.Result;
        };

        _hpFill.fillAmount = _controller.Model.CurrentEnemyStats.HP / _controller.GetEnemyStats().HP;
        _dragonAndHPText.text = _dragonName.ToUpper() + " " + "(" + _controller.Model.CurrentEnemyStats.HP + " / " + _controller.GetEnemyStats().HP + ")";
    }

    private float SetFillAmount()
    {
        float qty = (float)_controller.Model.CurrentEnemyStats.HP / (float)_controller.GetEnemyStats().HP;
        if (qty <= 0f) return 0f;
        else return qty;
    }

    private void OnDestroy()
    {
        _onPlayerAttacked.RemoveListener(TranslateAttackToDamage);
        _onMovesAvailableChanged.RemoveListener(OnMovesAreOver);
        _controller.OnHPChanged -= ChangeHP;
    }
}
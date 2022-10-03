using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour
{
    #region UI VARIABLES
    [SerializeField]
    private List<Sprite> _dragonSprites = new List<Sprite>();

    [SerializeField]
    private Image _dragonImage = null;

    [SerializeField]
    private Image _hpFill = null;

    [SerializeField]
    private TMP_Text _dragonAndHPText = null;
    private string _dragonName = "";
    #endregion

    EnemyController _controller;

    StatIntIntArgument_Event _onPlayerAttacked;

    public void Initialize(EnemyController enemyController, StatIntIntArgument_Event OnPlayerAttacked)
    {
        enemyController.OnHPChanged += ChangeHP;
        _onPlayerAttacked = OnPlayerAttacked;
        _onPlayerAttacked.AddListener(TranslateAttackToDamage);

        _controller = enemyController;

        SetStartingVisuals();
    }

    private void ChangeHP(int hp)
    {
        _hpFill.fillAmount = SetFillAmount();
        _dragonAndHPText.text = _dragonName.ToUpper() + " " + "(" + hp + " / " + _controller.GetEnemyStats().HP + ")";
    }

    private void TranslateAttackToDamage(Stats heroStats, int hits, int heroColor)
    {
        _controller.RecieveDamageFromPlayer(heroStats, hits, heroColor);
    }

    private void SetStartingVisuals()
    {
        _dragonName = _controller.Model.Enemy.Name;
        _dragonImage.sprite = _dragonSprites.Find(sprite => sprite.name == _controller.Model.Enemy.Id);
        _hpFill.fillAmount = _controller.Model.CurrentEnemyStats.HP / _controller.GetEnemyStats().HP;
        _dragonAndHPText.text = _dragonName.ToUpper() + " " + "(" + _controller.Model.CurrentEnemyStats.HP +" / " + _controller.GetEnemyStats().HP +")";
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
    }
}
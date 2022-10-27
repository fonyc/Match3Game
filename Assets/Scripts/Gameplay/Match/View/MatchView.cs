using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MatchView : MonoBehaviour
{
    private NoArgument_Event _onPlayerDeath;
    private NoArgument_Event _onEnemyDeath;
    private NoArgument_Event _onBuffReset;
    private NoArgument_Event _onPlayerRecievedDamage;
    private StringArgument_Event _onInputChange;

    private MatchController _matchController;

    private bool _matchEnded = false;

    [SerializeField] private GameObject _winPanelPrefab;
    [SerializeField] private GameObject _losePanelPrefab;
    [SerializeField] private RoundReportView _roundPanelPrefab;
    [SerializeField] private Button _addButton;


    private MatchReport _matchReport;

    public void Initialize(MatchController matchController, NoArgument_Event OnPlayerDeath,
        NoArgument_Event OnEnemyDeath, NoArgument_Event OnPlayerRecievedDamage, MatchReport matchReport,
        NoArgument_Event OnBuffsReset, StringArgument_Event OnInputDisabled
        )
    {
        _onInputChange = OnInputDisabled;

        _onBuffReset = OnBuffsReset;
        _matchReport = matchReport;
        _matchController = matchController;

        _onPlayerRecievedDamage = OnPlayerRecievedDamage;
        _onPlayerRecievedDamage.AddListener(OnRoundOver);

        _onPlayerDeath = OnPlayerDeath;
        _onEnemyDeath = OnEnemyDeath;

        _matchController.OnAddWatched += DisableAddButton;
        _onPlayerDeath.AddListener(OnPlayerLose);
        _onEnemyDeath.AddListener(OnPlayerWins);

        _roundPanelPrefab.SetData(_matchReport);
    }

    private void OnDestroy()
    {
        _onPlayerDeath.RemoveListener(OnPlayerLose);
        _onEnemyDeath.RemoveListener(OnPlayerWins);
        _onPlayerRecievedDamage.RemoveListener(OnRoundOver);
        _matchController.OnAddWatched -= DisableAddButton;
    }

    private void OnRoundOver()
    {
        _roundPanelPrefab.UpdateVisuals();
        StartCoroutine(AnimateInRoundPanel_Coro(_roundPanelPrefab.GetComponent<RectTransform>(), 0.75f));
    }

    #region ANIMATIONS

    public IEnumerator AnimateInRoundPanel_Coro(RectTransform rect, float delay)
    {
        yield return new WaitForSeconds(delay);
        rect.DOAnchorPos(Vector2.zero, 0.3f).SetEase(Ease.OutBack);
    }

    public void AnimateOutRoundPanel(RectTransform rect)
    {
        rect.DOAnchorPos(new Vector2(-2500f, 0), 0.25f).SetEase(Ease.InBack);
    }

    public void CloseRoundPanel()
    {
        AnimateOutRoundPanel(_roundPanelPrefab.GetComponent<RectTransform>());
        _matchReport.ClearReport();
        _onBuffReset.TriggerEvents();
    }

    #endregion

    private void OnPlayerWins()
    {
        if (_matchEnded) return;
        _matchEnded = true;
        _winPanelPrefab.SetActive(true);
        _matchController.GrantRewards();
        _onInputChange.TriggerEvents("NoInput");
    }

    public void ShowAd()
    {
        _matchController.GrantAddReward();
    }

    private void DisableAddButton()
    {
        _addButton.interactable = false;
    }

    private void OnPlayerLose()
    {
        if (_matchEnded) return;
        _matchEnded = true;
        _losePanelPrefab.SetActive(true);
        _onInputChange.TriggerEvents("NoInput");
    }

    public void GoBackToMainMenu()
    {
        _matchController.GoToMainMenu();
    }
}

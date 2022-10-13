using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MatchView : MonoBehaviour
{
    private NoArgument_Event _onPlayerDeath;
    private NoArgument_Event _onEnemyDeath;

    private MatchController _matchController;

    [SerializeField] private GameObject _winPanelPrefab;
    [SerializeField] private GameObject _losePanelPrefab;
    [SerializeField] private GameObject _roundPanelPrefab;
    [SerializeField] private Button _addButton;

    private NoArgument_Event _onPlayerRecievedDamage;

    public void Initialize(MatchController matchController, NoArgument_Event OnPlayerDeath,
        NoArgument_Event OnEnemyDeath, NoArgument_Event OnPlayerRecievedDamage)
    {
        _matchController = matchController;

        _onPlayerRecievedDamage = OnPlayerRecievedDamage;
        _onPlayerRecievedDamage.AddListener(OnRoundOver);

        _onPlayerDeath = OnPlayerDeath;
        _onEnemyDeath = OnEnemyDeath;

        _matchController.OnAddWatched += DisableAddButton;
        _onPlayerDeath.AddListener(OnPlayerLose);
        _onEnemyDeath.AddListener(OnPlayerWins);
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
        //Ask for data to the controller
        //Fill the data in the panel
        StartCoroutine(AnimateInRoundPanel_Coro(_roundPanelPrefab.GetComponent<RectTransform>(), 0.75f));
        //Reset model
    }

    #region ANIMATIONS

    public IEnumerator AnimateInRoundPanel_Coro(RectTransform rect, float delay)
    {
        _roundPanelPrefab.SetActive(true);
        yield return new WaitForSeconds(delay);
        rect.DOAnchorPos(Vector2.zero, 0.3f).SetEase(Ease.OutBack);
    }

    public void AnimateOutRoundPanel(RectTransform rect)
    {
        rect.DOAnchorPos(new Vector2(-2500f, 0), 0.25f).SetEase(Ease.InBack).OnComplete(HideRoundPanel);
    }

    private void HideRoundPanel()
    {
        _roundPanelPrefab.SetActive(false);
    }

    public void CloseRoundPanel()
    {
        AnimateOutRoundPanel(_roundPanelPrefab.GetComponent<RectTransform>());
    }

    #endregion

    private void OnPlayerWins()
    {
        _winPanelPrefab.SetActive(true);
        _matchController.GrantRewards();
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
        _losePanelPrefab.SetActive(true);
    }

    public void GoBackToMainMenu()
    {
        _matchController.GoToMainMenu();
    }
}

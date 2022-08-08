using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyHP_txt;
    [SerializeField] private TextMeshProUGUI playerHP_txt;
    [SerializeField] private TextMeshProUGUI enemyTurns_txt;

    [SerializeField] RectTransform enemyFill;
    [SerializeField] RectTransform playerFill;

    [SerializeField] RectTransform crossManaFill;
    [SerializeField] RectTransform horizontalManaFill;
    [SerializeField] RectTransform verticalManaFill;

    [SerializeField] GameObject crossShine;
    [SerializeField] GameObject horizontalShine;
    [SerializeField] GameObject verticalShine;

    #region EVENTS
    [SerializeField] private NoArgument_Event _OnBossDied;
    [SerializeField] private NoArgument_Event _OnPlayerDied;

    [SerializeField] private BoolArgument_Event _OnCrossManaSkillReady;
    [SerializeField] private BoolArgument_Event _OnVerticalManaSkillReady;
    [SerializeField] private BoolArgument_Event _OnHorizontalManaSkillReady;

    [SerializeField] private DoubleIntArgument_Event _OnPlayerHealthChanged;
    [SerializeField] private DoubleIntArgument_Event _OnEnemyHealthChanged;
    [SerializeField] private IntArgument_Event _OnEnemyTurnsChanged;

    [SerializeField] private DoubleIntArgument_Event _OnPlayerCrossManaChanged;
    [SerializeField] private DoubleIntArgument_Event _OnPlayerVerticalManaChanged;
    [SerializeField] private DoubleIntArgument_Event _OnPlayerHorizontalManaChanged;
    #endregion

    private void Awake()
    {
        _OnPlayerHealthChanged.AddListener(UpdatePlayerHealth);
        _OnEnemyHealthChanged.AddListener(UpdateEnemyHealth);
        _OnEnemyTurnsChanged.AddListener(UpdateEnemyTurns);

        _OnCrossManaSkillReady.AddListener(ActivateCrossShine);
        _OnVerticalManaSkillReady.AddListener(ActivateVerticalShine);
        _OnHorizontalManaSkillReady.AddListener(ActivateHorizontalShine);

        _OnPlayerCrossManaChanged.AddListener(UpdatePlayerCrossMana);
        _OnPlayerVerticalManaChanged.AddListener(UpdatePlayerVerticalMana);
        _OnPlayerHorizontalManaChanged.AddListener(UpdatePlayerHorizontalMana);
    }

    public void UpdatePlayerHealth(int hp, int max)
    {
        playerHP_txt.text = hp.ToString() + " / " + max.ToString();
        float scaleX = (float) hp / (float)max;
        playerFill.localScale = new Vector3(scaleX, playerFill.localScale.y, playerFill.localScale.z);
    }

    public void UpdateEnemyHealth(int hp, int max)
    {
        enemyHP_txt.text = hp.ToString() + " / " + max.ToString();
        float scaleX = (float)hp / (float)max;
        enemyFill.localScale = new Vector3(scaleX, playerFill.localScale.y, playerFill.localScale.z);
    }

    public void UpdateEnemyTurns(int currentTurns)
    {
        enemyTurns_txt.text = currentTurns.ToString();
    }

    public void UpdatePlayerCrossMana(int mana, int maxMana)
    {
        float scaleX = (float)mana / (float)maxMana;
        crossManaFill.localScale = new Vector3(scaleX, playerFill.localScale.y, playerFill.localScale.z);
    }

    public void UpdatePlayerHorizontalMana(int mana, int maxMana)
    {
        float scaleX = (float)mana / (float)maxMana;
        horizontalManaFill.localScale = new Vector3(scaleX, playerFill.localScale.y, playerFill.localScale.z);
    }

    public void UpdatePlayerVerticalMana(int mana, int maxMana)
    {
        float scaleX = (float)mana / (float)maxMana;
        verticalManaFill.localScale = new Vector3(scaleX, playerFill.localScale.y, playerFill.localScale.z);
    }

    public void ActivateCrossShine(bool value)
    {
        crossShine.SetActive(value);
    }

    public void ActivateVerticalShine(bool value)
    {
        verticalShine.SetActive(value);
    }

    public void ActivateHorizontalShine(bool value)
    {
        horizontalShine.SetActive(value);
    }

    private void OnDestroy()
    {
        _OnPlayerHealthChanged.RemoveListener(UpdatePlayerHealth);
        _OnEnemyHealthChanged.RemoveListener(UpdateEnemyHealth);
        _OnEnemyTurnsChanged.RemoveListener(UpdateEnemyTurns);

        _OnCrossManaSkillReady.RemoveListener(ActivateCrossShine);
        _OnVerticalManaSkillReady.RemoveListener(ActivateVerticalShine);
        _OnHorizontalManaSkillReady.RemoveListener(ActivateHorizontalShine);

        _OnPlayerCrossManaChanged.RemoveListener(UpdatePlayerCrossMana);
        _OnPlayerVerticalManaChanged.RemoveListener(UpdatePlayerVerticalMana);
        _OnPlayerHorizontalManaChanged.RemoveListener(UpdatePlayerHorizontalMana);
    }
}

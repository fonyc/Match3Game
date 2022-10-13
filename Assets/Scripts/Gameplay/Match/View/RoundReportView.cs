using TMPro;
using UnityEngine;
public class RoundReportView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _matchedEmblems = null;
    
    [SerializeField]
    private TMP_Text _bonusColumn = null;

    [SerializeField]
    private TMP_Text _maxCombo = null;

    [SerializeField]
    private TMP_Text _damageRecieved = null;

    [SerializeField]
    private TMP_Text _totalDamage = null;

    private MatchReport _matchReport;

    public void SetData(MatchReport matchReport)
    {
        _matchReport = matchReport;
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        _matchedEmblems.text = "matched emblems: " + _matchReport.matchedEmblems.ToString();
        _bonusColumn.text = "bonus column: " + _matchReport.columnsDestroyed.ToString();
        _maxCombo.text = "max. combo: " + _matchReport.maxCombo.ToString();
        _damageRecieved.text = "damage recieved: " + _matchReport.damageRecieved.ToString();
        _totalDamage.text = "total damage: " + _matchReport.damageDealt.ToString();
    }
}

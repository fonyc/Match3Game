using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _goldText;

    [SerializeField]
    private TMP_Text _gemsText;

    private GameProgressionService _gameProgression;

    public void Initialize(GameProgressionService userData)
    {
        _gameProgression = userData;
        _gameProgression.OnResourceModified += UpdateResourceView;
        UpdateViewData();
    }

    private void UpdateViewData()
    {
        UpdateResourceView("Gold");
        UpdateResourceView("Gems");
    }

    private void UpdateResourceView(string resource)
    {
        switch (resource)
        {
            case "Gold":
                _goldText.text = _gameProgression.GetResourceAmount("Gold").ToString();
                break;
            case "Gems":
                _gemsText.text = _gameProgression.GetResourceAmount("Gems").ToString();
                break;
        }
    }
}

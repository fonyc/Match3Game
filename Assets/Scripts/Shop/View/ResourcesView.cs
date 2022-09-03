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

    private UserData _userData;

    public void Initialize(UserData userData)
    {
        _userData = userData;
        _userData.OnResourceModified += UpdateResourceView;
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
                _goldText.text = _userData.GetResourceAmount("Gold").ToString();
                break;
            case "Gems":
                _gemsText.text = _userData.GetResourceAmount("Gems").ToString();
                break;
        }
    }
}

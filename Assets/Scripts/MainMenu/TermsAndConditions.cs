using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TermsAndConditions : MonoBehaviour
{
    GameConfigService _gameConfigService;
    private bool _termsReaded => PlayerPrefs.GetInt("termsAndConditions", 0) == 1;

    public void Initialize(GameConfigService gameConfigService)
    {
        _gameConfigService = gameConfigService;
        if (_termsReaded || _gameConfigService == null) Destroy(gameObject);
    }

    public void OpenURL()
    {
        Application.OpenURL(_gameConfigService.termsAndConditions);
    }

    public void Accept()
    {
        PlayerPrefs.SetInt("termsAndConditions", 1);
        ClosePanel();
    }

    public void Decline()
    {
        ClosePanel();
    }

    public void ClosePanel()
    {
        if (!_termsReaded) Application.Quit();
        else Destroy(gameObject);
    }
}

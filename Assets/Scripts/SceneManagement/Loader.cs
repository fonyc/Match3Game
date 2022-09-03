using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] Image progressImage;
    
    private void Start()
    {
        canvas.worldCamera = Camera.main;    
    }

    public void SetProgressText(string text) => progressText.text = text;
    public void SetProgressImage(float progress) => progressImage.fillAmount = progress;
    public float GetProgressImage() => progressImage.fillAmount;
}

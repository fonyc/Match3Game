using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    void Start()
    {
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Screen.width / Screen.height; // cameraHeight * aspect ratio

        transform.localScale = new Vector3(cameraHeight / 8.0f, cameraHeight / 8.0f, 1f);

    }
}

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class Screenshot
{
    [MenuItem("Tools/Screenshot/Take")]
    public static void TakeScreenShot()
    {
        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop),"Screenshot.png"));
    }
}
#endif
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;

    public void ChangeScene(int index)
    {
        StartCoroutine(LoadSceneAsync(index));
    }

    private IEnumerator LoadSceneAsync(int buildIndex)
    {
        float progress = 0f;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);

        loadingScreen.SetActive(true);
        Loader loader = loadingScreen.GetComponent<Loader>();

        asyncLoad.allowSceneActivation = false;

        while (progress <= 1f)
        {
            loader.SetProgressImage(progress);
            loader.SetProgressText(Mathf.Round(progress * 100f) + "%");

            progress += .01f;

            yield return new WaitForSeconds(.01f);
        }

        while (!asyncLoad.isDone && progress >= 1f)
        {
            asyncLoad.allowSceneActivation = true;
            yield return null;
        }
    }
}

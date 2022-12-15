using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{

    private GameObject loadingScreen;
    private Slider loadingSlider;
    private void Start()
    {
        loadingScreen = UIManager.instance.UI.transform.Find("LoadingScreen").gameObject;
        loadingSlider = loadingScreen.transform.Find("Slider").gameObject.GetComponent<Slider>();
    }
    public void LoadLevel(int sceneIndex)
    {
       StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;

            yield return null;
        }
    }
}

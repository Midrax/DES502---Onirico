using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{

    public GameObject loadingScreen;
    public GameObject butterflies;
    public Slider slider;
    public void LoadLevel (int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        loadingScreen.SetActive(true);
        butterflies.SetActive(false);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            //float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = operation.progress;

            yield return null;
        }
    
    }


}

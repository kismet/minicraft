using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AScyncLoader : MonoBehaviour
{
    [Header("Menu screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject playerUI;

    public string targetSceneName;


    public void LoadLevel(string levelToLoad)
    {
        playerUI.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !string.IsNullOrEmpty(targetSceneName))
        {
            LoadLevel(targetSceneName);
        }
    }

    IEnumerator LoadLevelASync(string levelToLoad)
    {


        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        float progressValue = 0;
        while (!loadOperation.isDone)
        {
            progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);

            if(progressValue >= 0.9f){
                loadOperation.allowSceneActivation = true;
            }
            
            yield return null;
        }
    }
}

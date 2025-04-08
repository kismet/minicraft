using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostLoadManager : MonoBehaviour
{
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject loadingScreen;

    void Start()
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        StartCoroutine(WaitForSceneReady());
    }

    IEnumerator WaitForSceneReady()
    {
        
        yield return null;

        yield return new WaitForSeconds(0.5f);

        if (loadingScreen != null)
            loadingScreen.SetActive(false);

        if (playerUI != null)
            playerUI.SetActive(true);
    }
}

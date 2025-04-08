using UnityEngine;

public class PostLoadManager : MonoBehaviour
{
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject loadingScreen;

    void Start()
    {
        if (playerUI != null)
            playerUI.SetActive(true);

        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }
}
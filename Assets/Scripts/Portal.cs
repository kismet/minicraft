using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string targetSceneName;

    // 1. Rendi il metodo PUBLIC
    public void LoadScene() 
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            UnityEngine.Debug.LogError("Nome scena non assegnato!");
        }
    }

    // Versione con collisione fisica
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            LoadScene();
        }
    }

    // O versione con trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadScene();
        }
    }
}
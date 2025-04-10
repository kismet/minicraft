using UnityEngine;

public class WarningMessage : MonoBehaviour
{
    [SerializeField] private GameObject warningMessage;

    void Start()
    {
        if (warningMessage != null)
            warningMessage.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && warningMessage != null)
        {
            warningMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && warningMessage != null)
        {
            warningMessage.SetActive(false);
        }
    }
}

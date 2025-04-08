using UnityEngine;

public class DiamondHardResetter : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteKey("DiamondCount"); // Cancella il salvataggio
        Debug.Log("Contatore diamanti resettato a zero!");
        
        // Se DiamondManager è già presente, resetta anche il suo valore
        if (DiamondManager.Instance != null)
        {
            DiamondManager.Instance.ResetDiamonds();
        }
    }
}
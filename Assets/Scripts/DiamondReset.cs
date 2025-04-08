using UnityEngine;

public class DiamondHardResetter : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteKey("DiamondCount");
        PlayerPrefs.DeleteKey("DesertDiamond");
        PlayerPrefs.DeleteKey("ForestDiamond");
        PlayerPrefs.DeleteKey("MesaDiamond");
        PlayerPrefs.DeleteKey("MountainDiamond");
        
        Debug.Log("Reset completato: diamanti e flag ripristinati!");

        if (DiamondManager.Instance != null)
        {
            DiamondManager.Instance.ResetDiamonds();
            DiamondManager.Instance.ResetAllDiamondFlags();
        }
    }
}
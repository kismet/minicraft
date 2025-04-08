using UnityEngine;

public class DiamondID : MonoBehaviour
{
    // Definisci un ID univoco per questo tipo di diamante.
    // Impostalo nell'Inspector per ogni prefab/istanza di diamante.
    // Esempi: "ForestDiamond", "DesertDiamond", "IceDiamond", "VolcanoDiamond"
    public string uniqueID;

    void Start()
    {
        // Validazione: Assicurati che un ID sia stato assegnato nell'Inspector
        if (string.IsNullOrEmpty(uniqueID))
        {
            Debug.LogError($"Il diamante '{gameObject.name}' non ha un uniqueID assegnato!", gameObject);
        }
    }
}

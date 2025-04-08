using UnityEngine;

public class DiamondID : MonoBehaviour
{
    // Definisci un ID numerico univoco per questo tipo di diamante.
    // Impostalo nell'Inspector per ogni prefab/istanza di diamante.
    // Esempio: 1 per Foresta, 2 per Deserto, 3 per Ghiaccio, 4 per Vulcano.
    // Assicurati che 0 non sia usato o sia considerato non valido se necessario.
    public int uniqueID;

    void Start()
    {
        // Validazione: Assicurati che un ID valido (es. > 0) sia stato assegnato
        if (uniqueID <= 0) // O qualsiasi altra logica di validazione (es. uniqueID == 0)
        {
            Debug.LogError($"Il diamante '{gameObject.name}' non ha un uniqueID valido assegnato (deve essere > 0)!", gameObject);
        }
    }
}

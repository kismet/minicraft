using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ToolbarUI : MonoBehaviour
{
    public UnityEngine.UI.Image[] slotHighlights; // Array delle immagini di evidenziazione degli slot
    private int selectedSlot = 0;  // Slot selezionato (0-8)

    void Start()
    {
        // Controlla che l'array sia stato assegnato correttamente
        if (slotHighlights == null || slotHighlights.Length == 0)
        {
            Debug.LogError("slotHighlights non � stato assegnato! Controlla l'Inspector.");
            return;
        }

        AggiornaEvidenziazione();
    }

    void Update()
    {
        GestisciSelezioneSlot();
    }

    void GestisciSelezioneSlot()
    {
        for (int i = 0; i < slotHighlights.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelezionaSlot(i);
            }
        }
    }
    
    void SelezionaSlot(int nuovoSlot)
    {
        if (nuovoSlot >= 0 && nuovoSlot < slotHighlights.Length)
        {
            selectedSlot = nuovoSlot;
            AggiornaEvidenziazione();
        }
    }

    void AggiornaEvidenziazione()
    {
        for (int i = 0; i < slotHighlights.Length; i++)
        {
            slotHighlights[i].enabled = (i == selectedSlot);
        }
    }
}

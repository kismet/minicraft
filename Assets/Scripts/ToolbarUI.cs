using UnityEngine;
using UnityEngine.UI;

public class ToolbarUI : MonoBehaviour
{
    [Header("Slot Highlights")]
    public Image[] slotHighlights; // Evidenziazione slot
    private int selectedSlot = 0;

    [Header("Block Icons (Selected)")]
    public GameObject[] blockIcons; // Immagini che rappresentano il blocco in mano

    void Start()
    {
        if (slotHighlights == null || slotHighlights.Length == 0)
        {
            Debug.LogError("slotHighlights non è stato assegnato! Controlla l'Inspector.");
            return;
        }

        if (blockIcons == null || blockIcons.Length == 0)
        {
            Debug.LogWarning("blockIcons non assegnato: verranno ignorate le icone a destra.");
        }
        else
        {
            DisattivaTutteLeIcone();
        }

        AggiornaEvidenziazione();
        AggiornaIconaSelezionata();
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
            AggiornaIconaSelezionata();
        }
    }

    void AggiornaEvidenziazione()
    {
        for (int i = 0; i < slotHighlights.Length; i++)
        {
            slotHighlights[i].enabled = (i == selectedSlot);
        }
    }

    void AggiornaIconaSelezionata()
    {
        if (blockIcons == null || blockIcons.Length == 0) return;

        for (int i = 0; i < blockIcons.Length; i++)
        {
            blockIcons[i].SetActive(i == selectedSlot);
        }
    }

    void DisattivaTutteLeIcone()
    {
        foreach (GameObject icon in blockIcons)
        {
            icon.SetActive(false);
        }
    }
}

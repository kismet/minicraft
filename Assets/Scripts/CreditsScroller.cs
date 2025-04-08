using UnityEngine;
using UnityEngine.UI; // Necessario se creditsContainer è un elemento UI
// using System.Collections; // Non più necessario per la coroutine
// using System.Diagnostics; // Rimosso se non serve per altro

public class CreditsScrollerInfinite : MonoBehaviour // Rinominato per chiarezza
{
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private RectTransform creditsContainer; // Il RectTransform che contiene i crediti
    [SerializeField] private float activationDelay = 2f; // Ritardo prima dell'inizio dello scroll

    private float startYPosition; // Posizione Y iniziale (appena sotto lo schermo)
    private float resetYPosition; // Posizione Y a cui resettare (quando la parte inferiore supera la parte superiore dello schermo)
    private float contentHeight;  // Altezza del contenitore dei crediti
    private float canvasHeight;   // Altezza del Canvas
    private bool canScroll = false; // Flag per gestire il ritardo iniziale

    void Start()
    {
        // È fondamentale che il Pivot del creditsContainer sia impostato correttamente.
        // Questo codice assume un pivot Y a 0.5 (centro).
        // Se il pivot è 1 (in alto) o 0 (in basso), i calcoli cambieranno.
        // Puoi controllare/impostare il Pivot nell'Inspector del RectTransform.
        if (creditsContainer.pivot.y != 0.5f)
        {
            UnityEngine.Debug.LogWarning("Il Pivot Y del RectTransform dei crediti non è 0.5 (centro). " +
                                         "Le posizioni potrebbero non essere calcolate correttamente. Pivot attuale: " + creditsContainer.pivot);
        }

        InitializePositions();

        // Disattiva lo scroll inizialmente e attivalo dopo il ritardo
        canScroll = false;
        Invoke(nameof(EnableScrolling), activationDelay);

        // Imposta la posizione iniziale
        Vector3 initialPos = creditsContainer.localPosition;
        initialPos.y = startYPosition;
        creditsContainer.localPosition = initialPos;
    }

    void InitializePositions()
    {
        // Trova il Canvas genitore
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            UnityEngine.Debug.LogError("CreditsScroller richiede un componente Canvas nella gerarchia dei suoi genitori.");
            enabled = false; // Disabilita lo script se non trova il canvas
            return;
        }
        canvasHeight = ((RectTransform)canvas.transform).rect.height;
        contentHeight = creditsContainer.rect.height;

        // Calcola la posizione Y iniziale (centro del contenuto appena sotto il bordo inferiore del canvas)
        // Questo posiziona il centro del RectTransform a metà altezza del contenuto sotto il bordo inferiore del canvas.
        startYPosition = -canvasHeight / 2f - contentHeight / 2f;

        // Calcola la posizione Y a cui deve avvenire il reset.
        // Questo succede quando il *centro* del contenuto raggiunge la posizione dove la *parte inferiore* del contenuto
        // ha appena superato il *bordo superiore* del canvas.
        resetYPosition = canvasHeight / 2f + contentHeight / 2f;
    }

    // Metodo chiamato da Invoke per abilitare lo scrolling
    void EnableScrolling()
    {
        canScroll = true;
    }

    void Update()
    {
        // Se lo scroll non è ancora abilitato, esci
        if (!canScroll)
        {
            return;
        }

        // Muovi il contenitore verso l'alto
        creditsContainer.Translate(Vector3.up * scrollSpeed * Time.deltaTime);

        // Controlla se il centro del contenitore ha superato la posizione di reset
        if (creditsContainer.localPosition.y >= resetYPosition)
        {
            // Resetta la posizione Y all'inizio (mantenendo la X e Z correnti)
            Vector3 resetPos = creditsContainer.localPosition;
            resetPos.y = startYPosition;
            creditsContainer.localPosition = resetPos;

            UnityEngine.Debug.Log("Crediti resettati all'inizio."); // Log opzionale per debug
        }
    }

    // OnCreditsFinished non è più necessario in un ciclo infinito
    // private void OnCreditsFinished() { ... }
}
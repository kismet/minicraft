using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;

public class CreditsScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private RectTransform creditsContainer;
    [SerializeField] private float activationDelay = 2f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float canvasHeight;

    private void Start()
    {
        InitializePositions();
        StartCoroutine(ScrollCredits());
    }

    private void InitializePositions()
    {
        canvasHeight = GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.height;

        // Imposta la posizione iniziale sotto il canvas
        startPosition = new Vector3(
            creditsContainer.localPosition.x,
            -canvasHeight,
            creditsContainer.localPosition.z
        );

        // Calcola la posizione finale sopra il canvas
        endPosition = new Vector3(
            startPosition.x,
            creditsContainer.rect.height + canvasHeight,
            startPosition.z
        );

        creditsContainer.localPosition = startPosition;
    }

    private IEnumerator ScrollCredits()
    {
        yield return new WaitForSeconds(activationDelay);

        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime * (scrollSpeed / creditsContainer.rect.height);
            creditsContainer.localPosition = Vector3.Lerp(startPosition, endPosition, progress);
            yield return null;
        }

        OnCreditsFinished();
    }

    private void OnCreditsFinished()
    {
        UnityEngine.Debug.Log("Crediti terminati!");
        // Aggiungi qui il codice per tornare al menu principale
        // Esempio: SceneManager.LoadScene("MainMenu");
    }
}
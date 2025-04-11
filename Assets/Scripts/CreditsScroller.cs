using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private RectTransform creditsContainer;
    [SerializeField] private float activationDelay = 2f;
    [SerializeField] private string finalSceneName = "FinalLevel"; // Nome della scena segreta

    private float startYPosition;
    private float endYPosition;
    private float contentHeight;
    private float canvasHeight;
    private bool canScroll = false;

    void Start()
    {
        if (creditsContainer.pivot.y != 0.5f)
        {
            Debug.LogWarning("Il Pivot Y del RectTransform dei crediti non è 0.5. Le posizioni potrebbero non essere corrette.");
        }

        InitializePositions();
        canScroll = false;
        Invoke(nameof(EnableScrolling), activationDelay);

        Vector3 initialPos = creditsContainer.localPosition;
        initialPos.y = startYPosition;
        creditsContainer.localPosition = initialPos;
    }

    void InitializePositions()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("CreditsScroller richiede un Canvas nella gerarchia dei genitori.");
            enabled = false;
            return;
        }

        canvasHeight = ((RectTransform)canvas.transform).rect.height;
        contentHeight = creditsContainer.rect.height;

        startYPosition = -canvasHeight / 2f - contentHeight / 2f;
        endYPosition = canvasHeight / 2f + contentHeight / 2f;
    }

    void EnableScrolling()
    {
        canScroll = true;
    }

    void Update()
    {
        if (!canScroll) return;

        creditsContainer.Translate(Vector3.up * scrollSpeed * Time.deltaTime);

        // Quando il centro del contenitore ha superato il bordo superiore, fine dello scroll
        if (creditsContainer.localPosition.y >= endYPosition)
        {
            canScroll = false;
            Debug.Log("Fine dei crediti. Caricamento della scena segreta...");
            SceneManager.LoadScene(finalSceneName);
        }
    }
}

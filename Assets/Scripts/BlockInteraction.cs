using UnityEngine;

public class BlockInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 4f;
    public Material highlightMaterial; // Materiale per evidenziare il blocco selezionato
    private Material originalMaterial; // Materiale originale del blocco
    private Transform selectedBlock; // Blocco attualmente selezionato
    private GameObject lastRemovedBlockPrefab; // Ultimo blocco rimosso

    void Update()
    {
        HandleBlockSelection();
        HandleBlockPlacement();
    }

    void HandleBlockSelection()
    {
        if (selectedBlock != null)
        {
            ResetBlockMaterial();
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("Block"))
            {
                selectedBlock = hit.transform;
                Renderer renderer = selectedBlock.GetComponent<Renderer>();

                if (renderer != null)
                {
                    originalMaterial = renderer.material;
                    renderer.material = highlightMaterial; // Applica il materiale evidenziato
                }
            }
        }
        else
        {
            selectedBlock = null;
        }
    }

    void HandleBlockPlacement()
    {
        if (Input.GetMouseButtonDown(0) && selectedBlock != null) // Rimuovere un blocco
        {
            print("Rimuovo blocco");
            lastRemovedBlockPrefab = selectedBlock.gameObject; // Memorizza il prefab rimosso
            Destroy(selectedBlock.gameObject);
        }

        if (Input.GetMouseButtonDown(1)) // Aggiungere un blocco
        {
            print("Piazzo blocco");
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance) && lastRemovedBlockPrefab != null)
            {
                Vector3 placePosition = hit.point + hit.normal * 0.5f;
                placePosition = new Vector3(Mathf.Round(placePosition.x), Mathf.Round(placePosition.y), Mathf.Round(placePosition.z));

                Instantiate(lastRemovedBlockPrefab, placePosition, Quaternion.identity);
            }
        }
    }

    void ResetBlockMaterial()
    {
        if (selectedBlock != null && originalMaterial != null)
        {
            selectedBlock.GetComponent<Renderer>().material = originalMaterial;
        }
    }

    void OnGUI()
    {
        float crosshairSize = 10f;
        float x = (Screen.width - crosshairSize) / 2;
        float y = (Screen.height - crosshairSize) / 2;
        
        GUI.Label(new Rect(x, y, crosshairSize, crosshairSize), "+");
    }
}

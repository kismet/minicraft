using UnityEngine;

public class BlockInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 4f;
    public Material highlightMaterial; // Materiale per evidenziare il blocco selezionato
    private Material originalMaterial; // Materiale originale del blocco
    private Transform selectedBlock; // Blocco attualmente selezionato
    public GameObject blockPrefab; // Assegna manualmente il prefab del blocco nell'Inspector


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

 /* GG void HandleBlockPlacement()
{
    if (Input.GetMouseButtonDown(0) && selectedBlock != null) // Rimuovere un blocco
    {
        print("Rimuovo blocco");
        Destroy(selectedBlock.gameObject);
    }

    if (Input.GetMouseButtonDown(1)) // Aggiungere un blocco
    {
        print("Piazzo blocco");
        if (blockPrefab != null)
        {
            if (selectedBlock != null)
            {
                // Piazzamento sopra il blocco selezionato (Y+1)
                Vector3 placePosition = selectedBlock.position + Vector3.up;
                Instantiate(blockPrefab, placePosition, Quaternion.identity);
            }
            else
            {
                // Piazzamento normale con il raggio
                Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, maxDistance))
                {
                    Vector3 placePosition = hit.point + hit.normal * 0.5f;
                    placePosition = new Vector3(Mathf.Round(placePosition.x), Mathf.Round(placePosition.y), Mathf.Round(placePosition.z));
                    Instantiate(blockPrefab, placePosition, Quaternion.identity);
                }
            }
        }
        else
        {
            Debug.Log("Nessun prefab di blocco assegnato. Assegna un prefab nell'Inspector.");
        }
    }
}*/

    void HandleBlockPlacement()
    {
        if (Input.GetMouseButtonDown(0) && selectedBlock != null) // Rimuovere un blocco
        {
            print("Rimuovo blocco");
            Destroy(selectedBlock.gameObject);
        }

        if (Input.GetMouseButtonDown(1)) // Aggiungere un blocco
        {
            print("Piazzo blocco");
            if (blockPrefab != null)
            {
                Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, maxDistance))
                {
                    // Usa la normale della superficie per determinare la posizione del nuovo blocco
                    Vector3 placePosition = hit.point + hit.normal * 0.5f; // Offset per evitare sovrapposizioni

                    // Allinea la posizione alla griglia
                    placePosition = new Vector3(Mathf.Round(placePosition.x), Mathf.Round(placePosition.y), Mathf.Round(placePosition.z));

                    // Verifica se la posizione è libera
                    if (!Physics.CheckSphere(placePosition, 0.1f)) // Controlla se c'è già un blocco
                    {
                        Instantiate(blockPrefab, placePosition, Quaternion.identity);
                    }
                    else
                    {
                        Debug.Log("Posizione occupata, non posso piazzare il blocco.");
                    }
                }
                else
                {
                    Debug.Log("Nessuna superficie colpita. Non posso piazzare il blocco.");
                }
            }
            else
            {
                Debug.Log("Nessun prefab di blocco assegnato. Assegna un prefab nell'Inspector.");
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
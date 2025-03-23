using UnityEngine;

public class BlockInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 4f;
    public GameObject blockPrefab; // Assegna manualmente il prefab del blocco nell'Inspector
    private Transform selectedBlock; // Blocco attualmente selezionato
    private Outline outlineEffect; // Riferimento allo script di outline

    void Update()
    {
        HandleBlockSelection();
        HandleBlockPlacement();
    }

    void HandleBlockSelection()
    {
        if (selectedBlock != null)
        {
            ResetBlockOutline();
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Block"))
            {
                selectedBlock = hit.transform;

                // Ottieni o aggiungi lo script Outline
                outlineEffect = selectedBlock.GetComponent<Outline>();
                if (outlineEffect == null)
                {
                    outlineEffect = selectedBlock.gameObject.AddComponent<Outline>();
                }

                // Attiva l'outline
                outlineEffect.enabled = true;
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
            if (selectedBlock.tag == "Bedrock")
            {
                Debug.Log("Non puoi rompere la Bedrock!");
                return;
            }

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
                    Vector3 placePosition = hit.point + hit.normal * 0.5f;
                    placePosition = new Vector3(Mathf.Round(placePosition.x), Mathf.Round(placePosition.y), Mathf.Round(placePosition.z));

                    if (!Physics.CheckSphere(placePosition, 0.1f))
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

    void ResetBlockOutline()
    {
        if (outlineEffect != null)
        {
            outlineEffect.enabled = false;
        }
    }
}
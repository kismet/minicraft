using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public class BlockInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 4f;
    public GameObject[] hotbarItems = new GameObject[9];
    public int currentSlotIndex = 0;

    private Transform selectedBlock;
    private Outline outlineEffect;
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    void Update()
    {
        HandleHotbarSelection();
        HandleBlockSelection();
        HandleBlockInteraction();
    }

    void HandleHotbarSelection()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0) currentSlotIndex = (currentSlotIndex + 1) % 9;
        if (scroll < 0) currentSlotIndex = (currentSlotIndex - 1 + 9) % 9;

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
                currentSlotIndex = i;
        }
    }

    void HandleBlockSelection()
    {
        if (selectedBlock != null)
        {
            ResetBlockOutline();
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Block"))
            {
                selectedBlock = hit.transform;
                outlineEffect = selectedBlock.GetComponent<Outline>();

                if (outlineEffect == null)
                    outlineEffect = selectedBlock.gameObject.AddComponent<Outline>();

                outlineEffect.enabled = true;
            }
        }
        else
        {
            selectedBlock = null;
        }
    }

    void HandleBlockInteraction()
    {
        // Distruzione blocchi e raccolta
        if (Input.GetMouseButtonDown(0) && currentSlotIndex == 0 && selectedBlock != null)
        {
            if (selectedBlock.CompareTag("Bedrock"))
            {
                UnityEngine.Debug.Log("Non puoi rompere la Bedrock!");
                return;
            }

            string blockType = selectedBlock.tag;
            AddToInventory(blockType, 1);
            Destroy(selectedBlock.gameObject);
            UnityEngine.Debug.Log($"Raccolto 1 {blockType}. Totale: {inventory[blockType]}");
        }

        // Piazzamento blocchi

        if (Input.GetMouseButtonDown(1))
        {
            // Solo slot 1-8 e prefab assegnato
            if (currentSlotIndex > 0 && hotbarItems[currentSlotIndex] != null)
            {
                string blockType = hotbarItems[currentSlotIndex].tag;

                if (HasBlocks(blockType))
                {
                    Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, maxDistance))
                    {
                        Vector3 placePosition = hit.point + hit.normal * 0.5f;
                        placePosition = new Vector3(
                            Mathf.Round(placePosition.x),
                            Mathf.Round(placePosition.y),
                            Mathf.Round(placePosition.z)
                        );

                        if (!Physics.CheckSphere(placePosition, 0.1f))
                        {
                            Instantiate(hotbarItems[currentSlotIndex], placePosition, Quaternion.identity);
                            RemoveFromInventory(blockType, 1);
                            UnityEngine.Debug.Log($"Piazzato {blockType}. Rimasti: {inventory[blockType]}");
                        }
                        else
                        {
                            UnityEngine.Debug.Log("Posizione occupata!");
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.Log("Nessuna superficie colpita!");
                    }
                }
                else
                {
                    UnityEngine.Debug.Log($"Non hai {blockType} nell'inventory!");
                }
            }
            else
            {
                UnityEngine.Debug.Log("Seleziona uno slot valido con un blocco!");
            }
        }
    }

    public void AddToInventory(string blockType, int quantity)
    {
        if (inventory.ContainsKey(blockType))
            inventory[blockType] += quantity;
        else
            inventory[blockType] = quantity;
    }

    bool HasBlocks(string blockType)
    {
        return inventory.ContainsKey(blockType) && inventory[blockType] > 0;
    }

    void RemoveFromInventory(string blockType, int quantity)
    {
        if (HasBlocks(blockType))
            inventory[blockType] = Mathf.Max(0, inventory[blockType] - quantity);
    }

    void ResetBlockOutline()
    {
        if (outlineEffect != null)
            outlineEffect.enabled = false;
    }

    // Metodo per debug dell'inventory
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 200), $"Slot selezionato: {currentSlotIndex}");
        int y = 30;
        foreach (var item in inventory)
        {
            GUI.Label(new Rect(10, y, 200, 20), $"{item.Key}: {item.Value}");
            y += 20;
        }
    }
}
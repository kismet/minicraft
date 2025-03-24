using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public class BlockInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 4f;
    public HotBarManager hotBarManager;

    private Transform selectedBlock;
    private Outline outlineEffect;
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    void Update()
    {
        HandleBlockSelection();
        HandleBlockInteraction();
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
        // Break blocks
        if (Input.GetMouseButtonDown(0))
        {
            if (hotBarManager.currentSlotIndex == 0 && selectedBlock != null)
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
        }

        // Place blocks
        if (Input.GetMouseButtonDown(1))
        {
            if (hotBarManager.currentSlotIndex > 0 &&
                hotBarManager.hotbarItems[hotBarManager.currentSlotIndex] != null)
            {
                GameObject blockToPlace = hotBarManager.hotbarItems[hotBarManager.currentSlotIndex];
                string blockType = blockToPlace.tag;

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
                            Instantiate(blockToPlace, placePosition, Quaternion.identity);
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

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 200), $"Slot selezionato: {hotBarManager.currentSlotIndex}");
        int y = 30;
        foreach (var item in inventory)
        {
            GUI.Label(new Rect(10, y, 200, 20), $"{item.Key}: {item.Value}");
            y += 20;
        }
    }
}
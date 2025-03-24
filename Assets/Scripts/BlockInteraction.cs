using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 4f;
    public HotBarManager hotBarManager;

    private Transform selectedBlock;
    private Outline outlineEffect;
    private bool isBreaking = false;
    private Coroutine breakCoroutine;

    void Update()
    {
        HandleBlockSelection();
        HandleBlockDestruction();
        HandleBlockPlacement();
    }

    void HandleBlockSelection()
    {
        if (selectedBlock != null)
            ResetBlockOutline();

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

    void HandleBlockDestruction()
    {
        if (Input.GetMouseButtonDown(0) && selectedBlock != null && selectedBlock.tag != "Bedrock")
        {
            if (!isBreaking && hotBarManager.currentSlotIndex == 0) // Controlla se il primo slot è selezionato
            {
                breakCoroutine = StartCoroutine(BreakBlock(selectedBlock));
            }
        }

        if (Input.GetMouseButtonUp(0) && isBreaking)
        {
            StopCoroutine(breakCoroutine);
            isBreaking = false;
        }
    }

    IEnumerator BreakBlock(Transform block)
    {
        isBreaking = true;
        Block blockData = block.GetComponent<Block>();
        float breakTime = blockData != null ? blockData.Durability : 1f;

        yield return new WaitForSeconds(breakTime);

        string blockType = block.tag;
        hotBarManager.AddToInventory(blockType, 1); // Aggiunge il blocco all'inventario
        Debug.Log("Rimozione Blocco " + blockType);
        Destroy(block.gameObject);
        isBreaking = false;
    }

    void HandleBlockPlacement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (hotBarManager.currentSlotIndex > 0 && hotBarManager.hotbarItems[hotBarManager.currentSlotIndex] != null)
            {
                GameObject blockToPlace = hotBarManager.hotbarItems[hotBarManager.currentSlotIndex];
                string blockType = blockToPlace.tag;

                if (hotBarManager.HasBlocks(blockType))
                {
                    Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, maxDistance))
                    {
                        Vector3 placePosition = hit.point + hit.normal * 0.5f;
                        placePosition = new Vector3(Mathf.Round(placePosition.x), Mathf.Round(placePosition.y), Mathf.Round(placePosition.z));

                        if (!Physics.CheckSphere(placePosition, 0.1f))
                        {
                            Instantiate(blockToPlace, placePosition, Quaternion.identity);
                            hotBarManager.RemoveFromInventory(blockType, 1);
                        }
                    }
                }
            }
        }
    }

    void ResetBlockOutline()
    {
        if (outlineEffect != null)
            outlineEffect.enabled = false;
    }
}
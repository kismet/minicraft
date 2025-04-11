using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BlockInteraction : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public HotBarManager hotBarManager;

    [Header("Settings")]
    public float maxDistance = 4f;
    public string currentScreen = "Game";
    public string diamondSceneName;

    private Transform selectedBlock;
    private Outline outlineEffect;

    void Update()
    {
        if (currentScreen == "Default") return;

        HandleBlockSelection();
        HandleBlockInteraction();    // Sinistro per distruggere
        HandleBlockPlacement();      // Destro per piazzare
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

    void HandleBlockInteraction()
    {
        //Tasto sinisto per spaccare
        if (Input.GetMouseButtonDown(0) && selectedBlock != null)
        {
            Block blockData = selectedBlock.GetComponent<Block>();

            if (blockData != null && blockData.IsBreakable)
            {
                string blockType = selectedBlock.tag;

                if (blockType == "Diamond")
                {
                    DiamondManager.Instance.AddDiamond();
                    Destroy(selectedBlock.gameObject);

                    if (!string.IsNullOrEmpty(diamondSceneName))
                    {
                        SceneManager.LoadScene(diamondSceneName);
                    }
                }
                else
                {
                    hotBarManager.AddToInventory(blockType, 1);
                    Destroy(selectedBlock.gameObject);
                }
            }
        }
    }

    void HandleBlockPlacement()
    {
        if (Input.GetMouseButtonDown(1)) // Tasto destro per posizionare
        {
            if (hotBarManager.hotbarItems[hotBarManager.currentSlotIndex] != null)
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
                        placePosition = new Vector3(
                            Mathf.Round(placePosition.x),
                            Mathf.Round(placePosition.y),
                            Mathf.Round(placePosition.z)
                        );

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

    public void SetCurrentScreen(string screen)
    {
        currentScreen = screen;
    }
}

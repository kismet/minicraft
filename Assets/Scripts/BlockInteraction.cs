using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Aggiungi questa riga per usare SceneManager

public class BlockInteraction : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public HotBarManager hotBarManager;
    
    [Header("Settings")]
    public float maxDistance = 4f;
    public string currentScreen = "Game";
    public string diamondSceneName; // Aggiungi questo campo per configurare la scena dall'Inspector

    private Transform selectedBlock;
    private Transform breakingBlock;
    private Outline outlineEffect;
    private bool isBreaking = false;
    private Coroutine breakCoroutine;

    void Start()
    {

    }

    void Update()
    {
        if (currentScreen == "Default") return;

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

        if (isBreaking && breakingBlock != selectedBlock)
        {
            StopBreaking();
        }
    }

    void HandleBlockDestruction()
    {
        if (currentScreen == "Default") return;

        if (Input.GetMouseButtonDown(0) && selectedBlock != null)
        {
            Block blockData = selectedBlock.GetComponent<Block>();

            if (blockData != null && blockData.IsBreakable && hotBarManager.currentSlotIndex == 0)
            {
                if (!isBreaking)
                {
                    breakingBlock = selectedBlock;
                    breakCoroutine = StartCoroutine(BreakBlock(selectedBlock));
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && isBreaking)
        {
            StopBreaking();
        }
    }

    IEnumerator BreakBlock(Transform block)
    {
        isBreaking = true;
        Block blockData = block.GetComponent<Block>();
        if (blockData == null || !blockData.IsBreakable)
        {
            StopBreaking();
            yield break;
        }

        float breakTime = blockData.Durability;
        float elapsedTime = 0f;
        string blockType = block.tag;

        while (elapsedTime < breakTime)
        {
            if (!isBreaking || breakingBlock != selectedBlock)
            {
                StopBreaking();
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (blockType == "Diamond")
        {
            
            // Distruggi il blocco
            Destroy(block.gameObject);
            
            // Carica la scena specificata
            if (!string.IsNullOrEmpty(diamondSceneName))
            {
                SceneManager.LoadScene(diamondSceneName);
            }
        }
        else
        {
            hotBarManager.AddToInventory(blockType, 1);
            Destroy(block.gameObject);
        }

        isBreaking = false;
    }

    void StopBreaking()
    {
        if (breakCoroutine != null)
        {
            StopCoroutine(breakCoroutine);
            breakCoroutine = null;
        }
        isBreaking = false;
        breakingBlock = null;
    }

    void HandleBlockPlacement()
    {
        if (currentScreen == "Default") return;

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
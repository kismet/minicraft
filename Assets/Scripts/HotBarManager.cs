using UnityEngine;
using TMPro;
using System.Reflection;
using System.Collections.Generic;

public class HotBarManager : MonoBehaviour
{
    public GameObject[] hotbarItems = new GameObject[9];
    public TMP_Text[] quantityTexts;
    public int currentSlotIndex = 0;

    private BlockInteraction blockInteraction;
    private Dictionary<string, int> inventory;

    void Start()
    {
        blockInteraction = FindObjectOfType<BlockInteraction>();
        FieldInfo fieldInfo = typeof(BlockInteraction).GetField("inventory",
            BindingFlags.NonPublic | BindingFlags.Instance);
        inventory = (Dictionary<string, int>)fieldInfo.GetValue(blockInteraction);
    }

    void Update()
    {
        HandleHotbarSelection();
        UpdateHotbarUI();
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

    void UpdateHotbarUI()
    {
        for (int i = 0; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] != null && i < quantityTexts.Length && quantityTexts[i] != null)
            {
                string blockType = hotbarItems[i].tag;
                int count = inventory != null && inventory.TryGetValue(blockType, out count) ? count : 0;
                quantityTexts[i].text = count.ToString();
            }
        }
    }
}
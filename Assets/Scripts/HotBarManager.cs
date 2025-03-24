using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HotBarManager : MonoBehaviour
{
    public GameObject[] hotbarItems = new GameObject[9]; // Slot della hotbar
    public TMP_Text[] quantityTexts; // Riferimenti UI per quantità dei blocchi
    public int currentSlotIndex = 0; // Slot attualmente selezionato
    public Dictionary<string, int> inventory = new Dictionary<string, int>(); // Inventario
    public int maxStackSize = 64; // Limite massimo di blocchi per tipo

    void Start()
    {
        InitializeInventory();
        UpdateHotbarUI();
        Block blockData = GetComponent<Block>();
    }

    void Update()
    {
        HandleHotbarSelection();
        UpdateHotbarUI();
    }

    void InitializeInventory()
    {
        inventory.Add("grass", 0);
        inventory.Add("dirt", 0);
        inventory.Add("log", 0);
        inventory.Add("stone", 0);
        inventory.Add("sand", 0);
        inventory.Add("terracotta", 0);
        inventory.Add("terracotta2", 0);
        inventory.Add("leaf", 0);
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

    public void AddToInventory(string blockType, int quantity)
    {
        Debug.Log($"Tentativo di aggiungere {quantity} {blockType} all'inventario.");

        if (inventory.ContainsKey(blockType))
        {
            if (inventory[blockType] < maxStackSize) // Limite massimo di 64
            {
                inventory[blockType] = Mathf.Min(64, inventory[blockType] + quantity);
            }
            else
            {
                Debug.Log("Inventario pieno per questo blocco!");
            }
        }
        else
        {
            inventory[blockType] = Mathf.Min(64, quantity);
        }

        Debug.Log($"Inventario aggiornato: {blockType} = {inventory[blockType]}");
    }

    public bool HasBlocks(string blockType)
    {
        return inventory.ContainsKey(blockType) && inventory[blockType] > 0;
    }

    public void RemoveFromInventory(string blockType, int quantity)
    {
        if (HasBlocks(blockType))
            inventory[blockType] = Mathf.Max(0, inventory[blockType] - quantity);
        
        UpdateHotbarUI();
    }

    void UpdateHotbarUI()
    {
        for (int i = 0; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] != null && i < quantityTexts.Length && quantityTexts[i] != null)
            {
                string blockType = hotbarItems[i].tag;
                int count = inventory.TryGetValue(blockType, out count) ? count : 0;
                quantityTexts[i].text = count.ToString();
            }
        }
    }
}
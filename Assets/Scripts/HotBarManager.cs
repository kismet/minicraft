using UnityEngine;

public class HotBarManager : MonoBehaviour
{
    public GameObject[] hotbarItems = new GameObject[9];
    public int currentSlotIndex = 0;

    void Update()
    {
        HandleHotbarSelection();
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
}
using UnityEngine;

[CreateAssetMenu(fileName = "DiamondCounter", menuName = "Inventory/DiamondCounter")]
public class DiamondCounterSO : ScriptableObject
{
    public int diamondCount = 0;

    public void AddDiamond()
    {
        diamondCount++;
        Debug.Log($"Diamanti totali: {diamondCount}");
    }

    public void ResetCounter()
    {
        diamondCount = 0;
    }
}
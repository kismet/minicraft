using UnityEngine;

public class HubPortalController : MonoBehaviour
{
    [Tooltip("Seleziona il tipo di diamante per questa scena")]
    public DiamondType sceneDiamondType;
    
    private void Update() 
    {
        // Controllo continuo dello stato del diamante
        if (DiamondManager.Instance == null) return;

        bool shouldDisablePortal = false;
        
        switch(sceneDiamondType)
        {
            case DiamondType.Desert:
                shouldDisablePortal = DiamondManager.Instance.DesertDiamond;
                break;
            case DiamondType.Forest:
                shouldDisablePortal = DiamondManager.Instance.ForestDiamond;
                break;
            case DiamondType.Mesa:
                shouldDisablePortal = DiamondManager.Instance.MesaDiamond;
                break;
            case DiamondType.Mountain:
                shouldDisablePortal = DiamondManager.Instance.MountainDiamond;
                break;
        }

        // Disattiva il portale se il diamante NON è stato raccolto
        gameObject.SetActive(!shouldDisablePortal);
    }
}

public enum DiamondType
{
    Desert,
    Forest,
    Mesa,
    Mountain
}
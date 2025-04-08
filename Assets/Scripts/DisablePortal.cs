using UnityEngine;

public class DisableChildrenBasedOnFlag : MonoBehaviour
{
    [Tooltip("Inserisci manualmente il nome della scena (Desert/Forest/Mesa/Mountain)")]
    public string sceneName; // Assegnalo dall'Inspector

    private void Start()
    {
        RefreshChildren();
    }

    public void RefreshChildren()
    {
        if (DiamondManager.Instance == null)
        {
            Debug.LogError("DiamondManager non trovato!");
            return;
        }

        bool shouldDisableChildren = false;

        switch (sceneName)
        {
            case "Desert":
                shouldDisableChildren = !DiamondManager.Instance.DesertDiamond;
                break;
            case "Forest":
                shouldDisableChildren = !DiamondManager.Instance.ForestDiamond;
                break;
            case "Mesa":
                shouldDisableChildren = !DiamondManager.Instance.MesaDiamond;
                break;
            case "Mountain":
                shouldDisableChildren = !DiamondManager.Instance.MountainDiamond;
                break;
            default:
                Debug.LogError("Nome scena non valido! Usa: Desert/Forest/Mesa/Mountain");
                return;
        }

        SetChildrenActive(!shouldDisableChildren);
    }

    private void SetChildrenActive(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }
}
using UnityEngine;

public class PortalActivator : MonoBehaviour
{
    public enum SceneDiamondType
    {
        Desert,
        Forest,
        Mesa,
        Mountain
    }

    [Tooltip("Seleziona quale diamante è associato alla scena in cui si trova questo portale.")]
    public SceneDiamondType associatedDiamond;

    private void Start()
    {
        UpdatePortalState(); // Aggiorna lo stato all'avvio della scena
    }

    public void UpdatePortalState()
    {
        if (DiamondManager.Instance == null)
        {
            Debug.LogError("DiamondManager.Instance non trovato! Impossibile aggiornare lo stato del portale.", this.gameObject);
            gameObject.SetActive(false);
            return;
        }

        bool shouldPortalBeActive = false;

        // Log di debug per verificare il flag corrente
        switch (associatedDiamond)
        {
            case SceneDiamondType.Desert:
                Debug.Log($"[PortalActivator] Scena: Desert, Flag in DiamondManager = {DiamondManager.Instance.DesertDiamond}");
                shouldPortalBeActive = !DiamondManager.Instance.DesertDiamond;
                break;
            case SceneDiamondType.Forest:
                Debug.Log($"[PortalActivator] Scena: Forest, Flag in DiamondManager = {DiamondManager.Instance.ForestDiamond}");
                shouldPortalBeActive = !DiamondManager.Instance.ForestDiamond;
                break;
            case SceneDiamondType.Mesa:
                Debug.Log($"[PortalActivator] Scena: Mesa, Flag in DiamondManager = {DiamondManager.Instance.MesaDiamond}");
                shouldPortalBeActive = !DiamondManager.Instance.MesaDiamond;
                break;
            case SceneDiamondType.Mountain:
                Debug.Log($"[PortalActivator] Scena: Mountain, Flag in DiamondManager = {DiamondManager.Instance.MountainDiamond}");
                shouldPortalBeActive = !DiamondManager.Instance.MountainDiamond;
                break;
            default:
                Debug.LogWarning($"Tipo di diamante non gestito: {associatedDiamond}", this.gameObject);
                shouldPortalBeActive = false;
                break;
        }

        Debug.Log($"[PortalActivator] {associatedDiamond} => Imposto il portale {(shouldPortalBeActive ? "ATTIVO" : "SPENTO")}");

        gameObject.SetActive(shouldPortalBeActive);
    }
}

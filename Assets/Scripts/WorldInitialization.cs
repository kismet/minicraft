using UnityEngine;

public class WorldInitialization : MonoBehaviour
{
    private void Start()
    {
        AllineaPosizioni(transform);
    }

    // Funzione ricorsiva per allineare le posizioni dei figli
    private void AllineaPosizioni(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Arrotonda la posizione del figlio
            Vector3 posizioneArrotondata = ArrotondaPosizione(child.position);

            // Imposta la nuova posizione
            child.position = posizioneArrotondata;

            // Richiama la funzione ricorsivamente per i figli dei figli
            AllineaPosizioni(child);
        }
    }

    // Funzione per arrotondare le coordinate x, y, z
private Vector3 ArrotondaPosizione(Vector3 posizione)
{
    float x = Mathf.Floor(posizione.x + 0.5f);
    float y = Mathf.Floor(posizione.y + 0.5f);
    float z = Mathf.Floor(posizione.z + 0.5f);

    return new Vector3(x, y, z);
}

}
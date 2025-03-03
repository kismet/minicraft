using UnityEngine;

public class caricamento_blocchi : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EliminaBlocchiCircondati(); // Correzione della chiamata del metodo
    }

    // Update is called once per frame
    void Update()
    {
        // Codice di aggiornamento se necessario
    }

    void EliminaBlocchiCircondati()
    {
        // Codice per eliminare i blocchi circondati
        GameObject[] blocchi = GameObject.FindGameObjectsWithTag("Cube");
        foreach (GameObject blocco in blocchi)
        {
            if (Circondato(blocco))
            {
                blocco.SetActive(false);// disattiva il blocco interessato
                Debug.Log("Blocco circondato eliminato");
            }
        }
    }

    bool Circondato(GameObject blocco)
    {
        // Codice per verificare se il blocco è circondato
        Vector3[] direzioni = {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        foreach (Vector3 direzione in direzioni)
        {
            RaycastHit hit;
            if (!Physics.Raycast(blocco.transform.position, direzione, out hit, 1.0f) || hit.collider.tag != "Cube")
            {
                return false;
            }
        }
        return true;
    }
}
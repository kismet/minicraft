using UnityEngine;
using UnityEditor;

public class ForestGenerator : MonoBehaviour
{
    public GameObject bedrockPrefab; // Prefab per la bedrock
    public GameObject stonePrefab; // Prefab per la pietra
    public GameObject dirtPrefab; // Prefab per la terra
    public GameObject grassPrefab; // Prefab per l'erba
    public GameObject treeTrunkPrefab; // Prefab per il tronco dell'albero
    public GameObject treeLeafPrefab; // Prefab per le foglie dell'albero
    public int chunkSize = 20; // Dimensione del chunk (20x20)
    public int height = 32; // Altezza massima del terreno
    public float terrainScale = 10f; // Scala del rumore di Perlin per l'altezza
    public float treeDensity = 0.1f; // Densità degli alberi (0 = nessuno, 1 = molto fitti)
    public int treeHeight = 5; // Altezza degli alberi
    public string prefabPath = "Assets/GeneratedForest.prefab"; // Percorso per salvare il prefab

    // Questo metodo viene chiamato automaticamente all'avvio del gioco
    private void Start()
    {
        GenerateAndSaveForest(); // Genera e salva la foresta
    }

    public void GenerateAndSaveForest()
    {
        // Crea un nuovo oggetto vuoto per la foresta
        GameObject forest = new GameObject("GeneratedForest");

        // Genera il terreno
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                // Crea la bedrock
                Instantiate(bedrockPrefab, new Vector3(x, 0, z), Quaternion.identity, forest.transform);

                // Calcola l'altezza usando il rumore di Perlin
                float xCoord = (float)x / chunkSize * terrainScale;
                float zCoord = (float)z / chunkSize * terrainScale;
                float noiseValue = Mathf.PerlinNoise(xCoord, zCoord);
                int terrainHeight = Mathf.FloorToInt(noiseValue * height);

                GameObject surfaceBlock = null; // Memorizza il blocco superficiale (erba)

                // Riempie il terreno con pietra, terra ed erba
                for (int y = 1; y <= terrainHeight; y++)
                {
                    GameObject blockPrefab;
                    if (y < terrainHeight - 4) // Strati più profondi: pietra
                    {
                        blockPrefab = stonePrefab;
                    }
                    else if (y < terrainHeight - 1) // Strati intermedi: terra
                    {
                        blockPrefab = dirtPrefab;
                    }
                    else // Strato superficiale: erba
                    {
                        blockPrefab = grassPrefab;
                        surfaceBlock = blockPrefab; // Memorizza il blocco superficiale
                    }

                    Instantiate(blockPrefab, new Vector3(x, y, z), Quaternion.identity, forest.transform);
                }

                // Aggiungi alberi in modo casuale
                if (surfaceBlock == grassPrefab && Random.Range(0f, 1f) < treeDensity)
                {
                    GenerateTree(x, terrainHeight + 1, z, forest.transform);
                }
            }
        }

        // Salva la foresta come prefab
        SaveForestAsPrefab(forest);
    }

    private void GenerateTree(int x, int y, int z, Transform parent)
    {
        // Crea il tronco dell'albero
        for (int i = 0; i < treeHeight; i++)
        {
            Instantiate(treeTrunkPrefab, new Vector3(x, y + i, z), Quaternion.identity, parent);
        }

        // Crea la chioma dell'albero (foglie)
        for (int dx = -2; dx <= 2; dx++)
        {
            for (int dz = -2; dz <= 2; dz++)
            {
                for (int dy = 0; dy < 3; dy++)
                {
                    // Crea uno strato di foglie
                    if (Mathf.Abs(dx) + Mathf.Abs(dz) <= 3) // Forma approssimativamente una sfera
                    {
                        Instantiate(treeLeafPrefab, new Vector3(x + dx, y + treeHeight - 1 + dy, z + dz), Quaternion.identity, parent);
                    }
                }
            }
        }
    }

    private void SaveForestAsPrefab(GameObject forest)
    {
#if UNITY_EDITOR
        PrefabUtility.SaveAsPrefabAsset(forest, prefabPath);
        DestroyImmediate(forest); // Distrugge l'oggetto nella scena dopo averlo salvato
        Debug.Log("Foresta salvata come prefab: " + prefabPath);
#endif
    }
}
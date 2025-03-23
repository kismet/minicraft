using UnityEngine;
using UnityEditor;

public class ChunkGenerator : MonoBehaviour
{
    public GameObject bedrockPrefab; // Prefab per la bedrock
    public GameObject terracottaType1; // Prefab per il primo tipo di terracotta
    public GameObject terracottaType2; // Prefab per il secondo tipo di terracotta
    public GameObject sandPrefab; // Prefab per la sabbia
    public GameObject stonePrefab; // Prefab per la pietra
    public int chunkSize = 20; // Dimensione del chunk (20x20)
    public int height = 32; // Altezza massima del terreno
    public float terrainScale = 10f; // Scala del rumore di Perlin per l'altezza
    public float sandScale = 20f; // Scala del rumore di Perlin per le macchie di sabbia (più alto = macchie più piccole)
    public string prefabPath = "Assets/GeneratedChunk.prefab"; // Percorso per salvare il prefab

    // Questo metodo viene chiamato automaticamente all'avvio del gioco
    private void Start()
    {
        GenerateAndSaveChunk(); // Genera e salva il chunk
    }

    public void GenerateAndSaveChunk()
    {
        // Crea un nuovo oggetto vuoto per il chunk
        GameObject chunk = new GameObject("GeneratedChunk");

        // Genera il terreno usando il rumore di Perlin
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                // Crea la bedrock
                Instantiate(bedrockPrefab, new Vector3(x, 0, z), Quaternion.identity, chunk.transform);

                // Calcola l'altezza usando il rumore di Perlin
                float xCoord = (float)x / chunkSize * terrainScale;
                float zCoord = (float)z / chunkSize * terrainScale;
                float noiseValue = Mathf.PerlinNoise(xCoord, zCoord);
                int terrainHeight = Mathf.FloorToInt(noiseValue * height);

                // Riempie il terreno con pietra, terracotta e sabbia
                for (int y = 1; y <= terrainHeight; y++)
                {
                    GameObject blockPrefab;

                    // Strati più profondi: pietra
                    if (y < terrainHeight - 3)
                    {
                        blockPrefab = stonePrefab;
                    }
                    // Strati superficiali: terracotta mista o sabbia
                    else
                    {
                        // Usa un secondo rumore di Perlin per decidere se posizionare la sabbia
                        float sandNoise = Mathf.PerlinNoise((float)x / chunkSize * sandScale, (float)z / chunkSize * sandScale);

                        // Se il valore di rumore è superiore a una soglia, posiziona la sabbia
                        if (sandNoise > 0.8f) // Soglia alta per macchie piccole e rare
                        {
                            blockPrefab = sandPrefab;
                        }
                        // Altrimenti, alterna tra i due tipi di terracotta
                        else
                        {
                            blockPrefab = Random.Range(0, 2) == 0 ? terracottaType1 : terracottaType2; // Mix casuale tra i due tipi
                        }
                    }

                    Instantiate(blockPrefab, new Vector3(x, y, z), Quaternion.identity, chunk.transform);
                }
            }
        }

        // Salva il chunk come prefab
        SaveChunkAsPrefab(chunk);
    }

    private void SaveChunkAsPrefab(GameObject chunk)
    {
#if UNITY_EDITOR
        PrefabUtility.SaveAsPrefabAsset(chunk, prefabPath);
        DestroyImmediate(chunk); // Distrugge l'oggetto nella scena dopo averlo salvato
        Debug.Log("Chunk salvato come prefab: " + prefabPath);
#endif
    }
}
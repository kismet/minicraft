using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject blockPrefab; // Prefab del blocco da utilizzare
    public int worldSizeX = 10; // Dimensione del mondo lungo l'asse X
    public int worldSizeZ = 10; // Dimensione del mondo lungo l'asse Z
    public int worldHeight = 3; // Altezza del mondo (livelli di blocchi)

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        for (int x = 0; x < worldSizeX; x++)
        {
            for (int z = 0; z < worldSizeZ; z++)
            {
                for (int y = 0; y < worldHeight; y++)
                {
                    Vector3 blockPosition = new Vector3(x, y, z);
                    Instantiate(blockPrefab, blockPosition, Quaternion.identity, transform);
                }
            }
        }
    }
}
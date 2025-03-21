using UnityEngine;

[ExecuteInEditMode]
public class BlockTexture : MonoBehaviour
{

    MeshFilter meshFilter;
    Mesh mesh;
    
    void Start() {
        
        meshFilter = GetComponent<MeshFilter>();

        mesh = meshFilter.sharedMesh;

        Vector2[] uv = mesh.uv;

        // front
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(.333f, 0);
        uv[2] = new Vector2(0, .333f);
        uv[3] = new Vector2(.333f, .333f);

        // top
        uv[4] = new Vector2(.334f, .333f);
        uv[5] = new Vector2(.666f, .333f);
        uv[8] = new Vector2(.334f, 0);
        uv[9] = new Vector2(.666f, 0);
        
        // back
        uv[6] = new Vector2(1, 0);
        uv[7] = new Vector2(.667f, 0);
        uv[10] = new Vector2(1, .333f);
        uv[11] = new Vector2(.667f, .333f);
        
        // bottom
        uv[12] = new Vector2(0, .334f);
        uv[13] = new Vector2(0, .666f);
        uv[14] = new Vector2(.333f, .666f);
        uv[15] = new Vector2(.333f, .334f);

        // left
        uv[16] = new Vector2(.334f, .334f);
        uv[17] = new Vector2(.334f, .666f);
        uv[18] = new Vector2(.666f, .666f);
        uv[19] = new Vector2(.666f, .334f);

        // right
        uv[20] = new Vector2(.667f, .334f);
        uv[21] = new Vector2(.667f, .666f);
        uv[22] = new Vector2(1, .666f);
        uv[23] = new Vector2(1, .334f);

        mesh.uv = uv;


    }

}
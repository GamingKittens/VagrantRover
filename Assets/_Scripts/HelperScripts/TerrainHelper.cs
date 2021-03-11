using UnityEngine;
using NaughtyAttributes;

public class TerrainHelper : MonoBehaviour
{
    #region Properties
    [Required]
    public Terrain terrain;

    public TerrainEditPresets preset;

    [HorizontalLine(color: EColor.Red)]
    [Header("HeightMap")]
    public TerrainEditMode mode;
    public float noiseScale = 10.0f;
    [MinMaxSlider(0,1)]
    public Vector2 heightRange = new Vector2(0, 1);
    [Min(1)]
    public float pow = 1;

    [HorizontalLine(color: EColor.Blue)]
    [Header("TextureMap")]
    public float ratio;
    [MinMaxSlider(0, 90)]
    public Vector2 cliffAngle;
    public int groundLayer1 = 0;
    public int groundLayer2 = 1;
    public int cliffLayer = 2;

    [HorizontalLine(color: EColor.Green)]
    [Header("Seed")]
    [Tooltip("Randomizes the offset values before generating new heightmaps")]
    public bool randomizeOffsets = true;
    [DisableIf("randomizeOffsets")]
    public Vector2 offset;

    [HorizontalLine(color: EColor.Indigo), Header("Mesh")]
    [Range(0,5), Tooltip("Higher values decrease resolution by 2^x")]
    public int scale = 1;
    public Material meshMat;
    private GameObject meshObj;
    
    private bool debug = true;
    #endregion Properties

    #region Editor Handles
    [Button("Apply Heightmap")]
    public void PerlinNoise ()
    {
        #region Debug
        System.DateTime startTime = System.DateTime.Now;
        #endregion Debug

        if (randomizeOffsets)
            RandomizeOffsets();

        switch (preset)
        {
            default:
            case TerrainEditPresets.Custom:
                GenerateHeights (terrain, mode, noiseScale, heightRange, offset);
                PowHeights      (terrain, pow);
                break;
            case TerrainEditPresets.Hills:
                GenerateHeights (terrain, TerrainEditMode.Set, noiseScale, new Vector2(0.5f, 1), offset);
                PowHeights      (terrain, 5);
                GenerateHeights (terrain, TerrainEditMode.Subtractive, noiseScale*2.5f, new Vector2(0.0f, 0.1f), offset*2);
                NormalizeHeights(terrain, heightRange.x, heightRange.y);
                break;
            case TerrainEditPresets.Plateaus:
                GenerateHeights (terrain, TerrainEditMode.Set, noiseScale, new Vector2(0, 1), offset);
                GenerateHeights (terrain, TerrainEditMode.Additive, noiseScale, new Vector2(0.25f, 0.4f), offset);
                PowHeights      (terrain, 20);
                GenerateHeights (terrain, TerrainEditMode.Additive, noiseScale*2.4f, new Vector2(0, 0.2f), offset);
                PowHeights      (terrain, 1.05f);
                NormalizeHeights(terrain, heightRange.x, heightRange.y);
                break;
            case TerrainEditPresets.Craters:
                GenerateHeights (terrain, TerrainEditMode.Set, noiseScale, new Vector2(0, 1), offset);
                GenerateHeights (terrain, TerrainEditMode.Additive, noiseScale, new Vector2(0.7f, 1), offset);
                NormalizeHeights(terrain, heightRange.x, heightRange.y);
                break;
        }

        #region Debug
        double elapsedTime = System.DateTime.Now.Subtract(startTime).TotalMilliseconds;
        if (debug)
            Debug.Log("Hieghtmap generation complete!\nElapsed time: " + elapsedTime + "ms");
        #endregion Debug
    }

    [Button("InvertHeights")]
    public void InvertHeightHandle()
    {
        InvertHeights(terrain);
    }

    [Button("ToggleMesh")]
    public void ToggleMesh()
    {
        if (meshObj)
        {
            DestroyImmediate(meshObj);
            terrain.enabled = true;
        }
        else
        {
            #region Debug
            System.DateTime startTime = System.DateTime.Now;
            #endregion Debug

            terrain.enabled = false;
            ConvertToMesh(terrain.terrainData);

            #region Debug
            double elapsedTime = System.DateTime.Now.Subtract(startTime).TotalMilliseconds;
            if (debug)
                Debug.Log("Mesh generation complete!\nElapsed time: " + elapsedTime + "ms");
            #endregion Debug
        }
    }
    #endregion Editor Handles

    #region Height Methods
    public void GenerateHeights(Terrain _terrain, TerrainEditMode _mode, float _tileSize, Vector2 _range, Vector2 _offset)
    {
        #region Sanitization
        if (_range.x >= _range.y)
        {
            Debug.Log("Warning, range.x cannot be larger than or equal to range.y\nOpperation not executed");
            return;
        }
        #endregion Sanitization

        int res = _terrain.terrainData.heightmapResolution;
        float[,] heights = _terrain.terrainData.GetHeights(0,0,res,res);
        float r = _range.y - _range.x;

        for (int i = 0; i < res; i++)
        {
            for (int k = 0; k < res; k++)
            {
                float p = Mathf.PerlinNoise(((_offset.y+i)/(float)res) * _tileSize, (_offset.x + k)/(float)res * _tileSize);

                p = (p*r) + _range.x; // Adjust for range

                switch (_mode)
                {
                    case TerrainEditMode.Set:
                        heights[i,k] = p;
                        break;
                    case TerrainEditMode.Additive:
                        heights[i,k] = Mathf.Clamp01(heights[i,k] + p);
                        break;
                    case TerrainEditMode.Subtractive:
                        heights[i,k] = Mathf.Clamp01(heights[i,k] - p);
                        break;
                    case TerrainEditMode.Multiplicative:
                        heights[i,k] = Mathf.Clamp01(heights[i,k] * p);
                        break;
                }
            }
        }

        _terrain.terrainData.SetHeights(0, 0, heights);
    }

    public void PowHeights (Terrain _terrain, float power)
    {
        #region Sanitization
        if (power <= 0)
        {
            Debug.LogWarning("power can not be negative!");
            return;
        }
        #endregion Sanitization

        int res = _terrain.terrainData.heightmapResolution;
        float[,] heights = _terrain.terrainData.GetHeights(0, 0, res, res);

        for (int i = 0; i < res; i++)
            for (int k = 0; k < res; k++)
                heights[i,k] = Mathf.Pow(heights[i,k], power);

        _terrain.terrainData.SetHeights(0, 0, heights);
    }

    public void InvertHeights (Terrain _terrain)
    {
        int res = _terrain.terrainData.heightmapResolution;
        float[,] heights = _terrain.terrainData.GetHeights(0, 0, res, res);

        for (int i = 0; i < res; i++)
            for (int k = 0; k < res; k++)
                heights[i,k] = 1 - heights[i,k];

        _terrain.terrainData.SetHeights(0, 0, heights);
    }

    public void NormalizeHeights(Terrain _terrain, float _min = 0, float _max = 1)
    {
        int res = _terrain.terrainData.heightmapResolution;
        float[,] heights = _terrain.terrainData.GetHeights(0, 0, res, res);

        float _min0 = 1;
        float _max0 = 0;

        for (int i = 0; i < res; i++)
            for (int k = 0; k < res; k++)
            {
                if (heights[i,k] > _max0)
                    _max0 = heights[i,k];
                if (heights[i,k] < _min0)
                    _min0 = heights[i,k];
            }

        float a = (_max - _min) / (_max0 - _min0);
        for (int i = 0; i < res; i++)
            for (int k = 0; k < res; k++)
                heights[i,k] = (heights[i,k]-_min0) * a + _min;

        _terrain.terrainData.SetHeights(0, 0, heights);
    }

    public void RandomizeOffsets ()
    {
        offset = new Vector2(Random.value, Random.value) * 10000;
    }
    #endregion Height Methods

    #region Texture Methods
    public void SlopeTextures (Terrain _terrain, Vector2 _angleRange, int _slopeTex)
    {
        int res = _terrain.terrainData.heightmapResolution;
        float[,,] map = _terrain.terrainData.GetAlphamaps(0,0,res,res);

        // For each point on the alphamap...
        for (int y = 0; y < res; y++)
        {
            for (int x = 0; x < res; x++)
            {
                // Get the normalized terrain coordinate that
                // corresponds to the the point.
                float normX = x / (res-1f);
                float normY = y / (res-1f);

                // Get the steepness value at the normalized coordinate.
                var angle = _terrain.terrainData.GetSteepness(normX, normY);

                // Steepness is given as an angle, 0..90 degrees. Divide
                // by 90 to get an alpha blending value in the range 0..1.
                ///TODO: Your mum
                var frac = angle / 90.0;
                map[x, y, 0] = (float)frac;
                map[x, y, 1] = (float)(1 - frac);
            }
        }
        _terrain.terrainData.SetAlphamaps(0, 0, map);
    }
    #endregion Texture Methods

    #region Mesh Methods
    public void ConvertToMesh (TerrainData td)
    {
        int fac = (int)Mathf.Pow(2, scale);
        int res = 1 + ((td.heightmapResolution-1) / fac);
        int vertCount = res * res;
        int trisCount = 2 * (res-1)*(res-1);

        Debug.Log("Resolution: " + (res-1) + " + 1\nVertices:" + vertCount);

        #region CreateObjects
        meshObj = new GameObject();
        meshObj.transform.parent = transform;
        meshObj.transform.localPosition = Vector3.zero;
        meshObj.name = "DebugMesh";
        MeshRenderer meshRenderer = meshObj.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = meshMat;
        MeshFilter meshFilter = meshObj.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[vertCount];
        int[] tris = new int[trisCount*3];
        Vector3[] normals = new Vector3[vertCount];
        Vector2[] uv = new Vector2[vertCount];
        #endregion CreateObjects

        // Sets verts to match position of terrain
        for (int i=0; i<vertCount; i++)
        {
            Vector2Int pos = GetPos(i, res);
            
            vertices[i] = new Vector3(
                td.size.x * pos.x/(res-1),
                td.GetHeight(pos.x*fac, pos.y*fac),
                td.size.z * pos.y/(res-1));
        }
        mesh.vertices = vertices;

        // Loops through each quad, setting flat tri values
        for (int i = 0; i < trisCount/2; i++)
        {
            Vector2Int pos = GetPos(i, res-1);
            int a = i * 6;

            // First tri - Bot Left
            tris[a+0] = GetVal(0, 0, pos, res);
            tris[a+1] = GetVal(0, 1, pos, res);
            tris[a+2] = GetVal(1, 0, pos, res);

            // Second tri - Top right
            tris[a+3] = GetVal(0, 1, pos, res);
            tris[a+4] = GetVal(1, 1, pos, res);
            tris[a+5] = GetVal(1, 0, pos, res);
        }
        mesh.triangles = tris;

        // Simple up-facing Normals
        for (int i = 0; i < vertCount; i++)
        {
            normals[i] = -Vector3.forward;
        }
        mesh.normals = normals;

        // IDRK how UVs work
        for (int i = 0; i < vertCount; i++)
        {
            Vector2Int pos = GetPos(i, res);

            uv[i] = new Vector2(pos.x, pos.y);
        }
        mesh.uv = uv;

        meshFilter.mesh = mesh;
    }

    private Vector2Int GetPos (int _i, int _res)
    {
        return new Vector2Int(_i % _res, _i / _res);
    }

    private int GetVal (int _x, int _y, Vector2Int _pos, int _res)
    {
        return (_pos.x+_x) + ((_pos.y+_y) * _res);
    }
    #endregion Mesh Methods
}

#region Enums
public enum TerrainEditMode
{
    Set,
    Additive,
    Subtractive,
    Multiplicative
}

public enum TerrainEditPresets
{
    Custom,
    Hills,
    Plateaus,
    Craters
}
#endregion Enums
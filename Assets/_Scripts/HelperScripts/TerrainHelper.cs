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
    public float terrainScale = 10.0f;
    [MinMaxSlider(0,1)]
    public Vector2 heightRange = new Vector2(0, 1);
    [Min(1)]
    public float pow = 1;

    [HorizontalLine(color: EColor.Blue)]
    [Header("AlphaMap")]
    public float alphaScale = 5f;
    [Range(0, 1)]
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
    public Vector2 offsetAlphas;

    [HorizontalLine(color: EColor.Indigo), Header("Mesh")]
    [Range(0,5), Tooltip("Higher values decrease resolution by 2^x")]
    public int scale = 1;
    public Material meshMat;
    private GameObject meshObj;
    
    private bool debug = true;
    #endregion Properties

    #region Editor Handles
    [Button("Apply")]
    public void Apply ()
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
                PerlinHeights   (terrain.terrainData, mode, terrainScale, heightRange, offset);
                PowHeights      (terrain.terrainData, pow);

                PerlinAlphas(terrain.terrainData, offsetAlphas, groundLayer1, groundLayer2, alphaScale, ratio);
                SlopeAlphas (terrain.terrainData, cliffAngle, cliffLayer);
                break;

            case TerrainEditPresets.Hills:
                PerlinHeights   (terrain.terrainData, TerrainEditMode.Set, terrainScale, new Vector2(0.5f, 1), offset);
                PowHeights      (terrain.terrainData, 5);
                PerlinHeights   (terrain.terrainData, TerrainEditMode.Subtractive, terrainScale * 2.5f, new Vector2(0.0f, 0.1f), offset*2);
                NormalizeHeights(terrain.terrainData, heightRange.x, heightRange.y);

                ClearAlphas (terrain.terrainData, 0);
                PerlinAlphas(terrain.terrainData, offsetAlphas, groundLayer1, groundLayer2, alphaScale, ratio);
                SlopeAlphas (terrain.terrainData, new Vector2(12.5f, 70), cliffLayer);
                break;

            case TerrainEditPresets.Plateaus:
                PerlinHeights   (terrain.terrainData, TerrainEditMode.Set, terrainScale, new Vector2(0, 1), offset);
                PerlinHeights   (terrain.terrainData, TerrainEditMode.Additive, terrainScale, new Vector2(0.25f, 0.4f), offset);
                PowHeights      (terrain.terrainData, 20);
                PerlinHeights   (terrain.terrainData, TerrainEditMode.Additive, terrainScale * 2.4f, new Vector2(0, 0.2f), offset);
                PowHeights      (terrain.terrainData, 1.05f);
                NormalizeHeights(terrain.terrainData, heightRange.x, heightRange.y);

                ClearAlphas (terrain.terrainData, 0);
                PerlinAlphas(terrain.terrainData, offsetAlphas, groundLayer1, groundLayer2, alphaScale, ratio);
                SlopeAlphas (terrain.terrainData, new Vector2(15, 45), cliffLayer);
                break;

            case TerrainEditPresets.Craters:
                PerlinHeights   (terrain.terrainData, TerrainEditMode.Set, terrainScale, new Vector2(0, 1), offset);
                PerlinHeights   (terrain.terrainData, TerrainEditMode.Additive, terrainScale, new Vector2(0.7f, 1), offset);
                NormalizeHeights(terrain.terrainData, heightRange.x, heightRange.y);

                ClearAlphas (terrain.terrainData, 0);
                PerlinAlphas(terrain.terrainData, offsetAlphas, groundLayer1, groundLayer2, alphaScale, ratio);
                SlopeAlphas (terrain.terrainData, new Vector2(0, 0.1f), cliffLayer);
                break;

            case TerrainEditPresets.Pillars:
                PerlinHeights   (terrain.terrainData, TerrainEditMode.Set, terrainScale, new Vector2(0, 1), offset);
                PerlinHeights   (terrain.terrainData, TerrainEditMode.Additive, terrainScale, new Vector2(0.7f, 1), offset);
                InvertHeights   (terrain.terrainData);
                PerlinHeights   (terrain.terrainData, TerrainEditMode.Additive, terrainScale/6, new Vector2(0, 0.04f), offset*2);
                PerlinHeights   (terrain.terrainData, TerrainEditMode.Additive, terrainScale/1.5f, new Vector2(0, 0.02f), offset*3);
                NormalizeHeights(terrain.terrainData, heightRange.x, heightRange.y);

                ClearAlphas (terrain.terrainData, 0);
                PerlinAlphas(terrain.terrainData, offsetAlphas, groundLayer1, groundLayer2, alphaScale, ratio);
                SlopeAlphas (terrain.terrainData, new Vector2(10, 15), cliffLayer);
                break;
        }

        #region Debug
        double elapsedTime = System.DateTime.Now.Subtract(startTime).TotalMilliseconds;
        if (debug)
            Debug.Log("Hieghtmap generation complete!\nElapsed time: " + elapsedTime + "ms");
        #endregion Debug
    }

    [Button("Invert Heights")]
    public void InvertHeightHandle()
    {
        InvertHeights(terrain.terrainData);
    }


    [Button("Textures Only")]
    public void TexturesOnly()
    {
        #region Debug
        System.DateTime startTime = System.DateTime.Now;
        #endregion Debug

        if (randomizeOffsets)
            RandomizeOffsets();

        PerlinAlphas(terrain.terrainData, offsetAlphas, groundLayer1, groundLayer2, alphaScale, ratio);
        SlopeAlphas(terrain.terrainData, cliffAngle, cliffLayer);

        #region Debug
        double elapsedTime = System.DateTime.Now.Subtract(startTime).TotalMilliseconds;
        if (debug)
            Debug.Log("Texturemap generation complete!\nElapsed time: " + elapsedTime + "ms");
        #endregion Debug
    }

    [Button("Clear Textures")]
    public void ClearAlphasHandler()
    {
        ClearAlphas(terrain.terrainData);
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
    public void PerlinHeights(TerrainData _td, TerrainEditMode _mode, float _tileSize, Vector2 _range, Vector2 _offset)
    {
        #region Sanitization
        if (_range.x > _range.y)
        {
            Debug.LogWarning("range.x cannot be larger than range.y\nOpperation not executed");
            return;
        }
        else if (_range.x == _range.y)
            return;
        #endregion Sanitization

        int res = _td.heightmapResolution;
        float[,] heights = _td.GetHeights(0,0,res,res);
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

        _td.SetHeights(0, 0, heights);
    }

    public void PowHeights (TerrainData _td, float power)
    {
        #region Sanitization
        if (power <= 0)
        {
            Debug.LogWarning("power can not be negative!\nOpperation not executed");
            return;
        }
        #endregion Sanitization

        int res = _td.heightmapResolution;
        float[,] heights = _td.GetHeights(0, 0, res, res);

        for (int i = 0; i < res; i++)
            for (int k = 0; k < res; k++)
                heights[i,k] = Mathf.Pow(heights[i,k], power);

        _td.SetHeights(0, 0, heights);
    }

    public void InvertHeights (TerrainData _td)
    {
        int res = _td.heightmapResolution;
        float[,] heights = _td.GetHeights(0, 0, res, res);

        for (int i = 0; i < res; i++)
            for (int k = 0; k < res; k++)
                heights[i,k] = 1 - heights[i,k];

        _td.SetHeights(0, 0, heights);
    }

    public void NormalizeHeights(TerrainData _td, float _min = 0, float _max = 1)
    {
        #region Sanitization
        if (_min >= _max)
        {
            Debug.LogWarning("_min cannot be greater than or equal to _max\nOpperation not executed");
            return;
        }
        #endregion Sanitization

        int res = _td.heightmapResolution;
        float[,] heights = _td.GetHeights(0, 0, res, res);

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

        _td.SetHeights(0, 0, heights);
    }

    public void RandomizeOffsets ()
    {
        offset = new Vector2(Random.value, Random.value) * 10000;
        offsetAlphas = new Vector2(Random.value, Random.value) * 10000;
    }
    #endregion Height Methods

    #region Alpha Methods

    public void PerlinAlphas (TerrainData _td, Vector2 _offset, int _layerA = 0, int _layerB = 1, float _tileSize = 1f, float _ratio = 0.5f)
    {
        #region Sanitization
        if (_ratio<0 || _ratio>1)
        {
            Debug.LogWarning("_ration must be between 0 and 1\n_ration given: "+_ratio);
            return;
        }
        if (_layerA<0 || _layerB<0)
            return;
        #endregion Sanitization

        int res = _td.alphamapResolution;
        float[,,] map = _td.GetAlphamaps(0, 0, res, res);

        for (int x = 0; x < res; x++)
        {
            for (int y = 0; y < res; y++)
            {
                //Don't forget to call mom
                float p = Mathf.PerlinNoise(_tileSize*(_offset.x+x)/res, _tileSize*(_offset.y+y)/res);
                if (_layerA != -1)
                    map[x,y,_layerA] = p;
                if (_layerB != -1)
                    map[x,y,_layerB] = 1-p;
            }
        }
        _td.SetAlphamaps(0, 0, map);
    }

    public void SlopeAlphas (TerrainData _td, Vector2 _angleRange, int _slopeLayer)
    {
        #region Sanitization
        if (_angleRange.x > _angleRange.y)
        {
            Debug.LogWarning("angleRange.x cannot be larger than angleRange.y\nOpperation not executed");
            return;
        }
        #endregion Sanitization

        int res = _td.alphamapResolution;
        float[,,] map = _td.GetAlphamaps(0,0,res,res);

        // For each point on the alphamap...
        for (int x = 0; x < res; x++)
        {
            for (int y = 0; y < res; y++)
            {
                float angle = _td.GetSteepness(y/(res-1f), x/(res-1f));
                float frac = 0;

                if (angle >= _angleRange.y)
                    frac = 1;
                else if (angle > _angleRange.x)
                    frac = (angle-_angleRange.x) / (_angleRange.y-_angleRange.x);

                for (int i = 0; i < map.GetLength(2); i++)
                    if (i == _slopeLayer)
                        map[x, y, i] = frac;
                    else
                        map[x, y, i] *= (1 - frac);
            }
        }
        _td.SetAlphamaps(0, 0, map);
    }

    public void ClearAlphas (TerrainData _td, int _fillLayer = -1)
    {
        float[,,] map = _td.GetAlphamaps(0, 0, _td.alphamapResolution, _td.alphamapResolution);

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
                for (int i = 0; i < map.GetLength(2); i++)
                    map[x, y, i] = i==_fillLayer?1:0;

        _td.SetAlphamaps(0, 0, map);
    }
    #endregion Alpha Methods

    #region Mesh Methods
    public void ConvertToMesh (TerrainData td)
    {
        int fac = (int)Mathf.Pow(2, scale);
        int res = 1 + ((td.heightmapResolution-1) / fac);
        int vertCount = res * res;
        int trisCount = 2 * (res-1)*(res-1);

        //Debug.Log("Resolution: " + (res-1) + " + 1\nVertices:" + vertCount);

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

        // Let Unity do the Normals for us
        mesh.RecalculateNormals();

        // UVs control how the texture is placed on the mesh
        for (int i = 0; i < vertCount; i++)
        {
            Vector2Int pos = GetPos(i, res);

            //uv[i] = new Vector2(pos.x/(res-1f), pos.y/(res-1f)); // Entire mesh is 1 tile
            uv[i] = new Vector2(pos.x, pos.y); // Each quad is 1 tile
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
    Craters,
    Pillars
}
#endregion Enums
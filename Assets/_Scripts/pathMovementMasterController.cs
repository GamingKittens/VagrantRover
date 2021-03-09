using NaughtyAttributes;
using UnityEngine;
public class PathMovementMasterController : MonoBehaviour
{
    #region Properties
    public GameObject pathMovementBase;
    public GameObject newPaths;
    public GameObject basePathExplorer;
    public float baseSpeed;
    public float baseSpeedAway;
    public float distanceAwayBase;
    [MinMaxSlider(0,100)][OnValueChanged("RoundFollowerCount")]
    public Vector2 followerCount;
    public RandomFromDistribution.ConfidenceLevel_e conf_level;
    int numPaths;
    Component[] basePathNodes;
    #endregion Properties

    #region Initialization
    void Start()
    {
        numPaths = (int)RandomFromDistribution.RandomRangeNormalDistribution(followerCount.x, followerCount.y, conf_level);
        basePathNodes = pathMovementBase.GetComponentsInChildren(typeof(Transform), includeInactive:true);

        for(int i = 0; i < numPaths; i++){
            GeneratePath();
        }
    }

    void GeneratePath()
    {
        GameObject newPath = new GameObject();
        for (int j = 0; j < basePathNodes.Length; j++)
        {
            // Later once you add in the array support for positons, you can replace this with a one-liner:
            //newPath.AddNode(basePathNodes[j].transform.position + GenerateOffsets());
            GameObject newNode = new GameObject();
            newNode.transform.position = basePathNodes[j].transform.position + GenerateOffsets();
            newNode.transform.parent = newPath.transform;
        }
        GameObject newPathExplorer = Instantiate(basePathExplorer);
        newPathExplorer.transform.parent = transform;
        newPathExplorer.transform.position = newPath.transform.GetChild(0).position; // Sets the followers position to that of it's starting node.
        PathMovement pm = newPath.AddComponent<PathMovement>();
        pm.pathExplorerCharacter = newPathExplorer;
        pm.moveSpeed = RandomFromDistribution.RandomRangeNormalDistribution(baseSpeed-baseSpeedAway, baseSpeed+baseSpeedAway, conf_level);
        newPath.transform.parent = newPaths.transform;
    }

    ///<summary>Generates a randomized offset from a node as a Vector3</summary>
    private Vector3 GenerateOffsets()
    {
        Vector3 offsets;
        offsets.x = RandomFromDistribution.RandomRangeNormalDistribution(-distanceAwayBase, +distanceAwayBase, conf_level);
        offsets.y = RandomFromDistribution.RandomRangeNormalDistribution(-distanceAwayBase, +distanceAwayBase, conf_level)/8f;
        offsets.z = RandomFromDistribution.RandomRangeNormalDistribution(-distanceAwayBase, +distanceAwayBase, conf_level);
        return offsets;
    }
    #endregion Initialization

    #region EditorHandles
    private void RoundFollowerCount ()
    {
        followerCount.x = Mathf.RoundToInt(followerCount.x);
        followerCount.y = Mathf.RoundToInt(followerCount.y);
    }
    #endregion EditorHandles
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class pathMovementMasterController : MonoBehaviour
{
    public GameObject pathMovementBase;
    public GameObject newPaths;
    public GameObject basePathExplorer;
    public float baseSpeed;
    public float baseSpeedAway;
    public float distanceAwayBase;
    public int min;
    public int max;
    public RandomFromDistribution.ConfidenceLevel_e conf_level;
    int numPaths;
       Component[] basePathNodes;

    // Start is called before the first frame update
    void Start()
    {
        numPaths = (int)RandomFromDistribution.RandomRangeNormalDistribution(min, max, conf_level);
        basePathNodes = pathMovementBase.GetComponentsInChildren(typeof(Transform), includeInactive:true);

        for(int i = 0; i < numPaths; i++){
            generatePath(i);
        }
    }

    void generatePath(int i)
    {
        GameObject newPath = new GameObject();
        float x,y,z;
        for (int j = 0; j < basePathNodes.Length; j++){
            x = basePathNodes[j].transform.position.x;
            y = basePathNodes[j].transform.position.y;
            z = basePathNodes[j].transform.position.z;
            x = RandomFromDistribution.RandomRangeNormalDistribution(x-distanceAwayBase, x+distanceAwayBase, conf_level);
            y = y + RandomFromDistribution.RandomRangeNormalDistribution(y-distanceAwayBase, y+distanceAwayBase, conf_level)/10;
            z = RandomFromDistribution.RandomRangeNormalDistribution(z-distanceAwayBase, z+distanceAwayBase, conf_level);
            GameObject newNode = new GameObject();
            newNode.transform.position = new Vector3(x,y,z);
            newNode.transform.parent = newPath.transform;
        }
        GameObject newPathExplorer = Instantiate(basePathExplorer);
        newPath.AddComponent<pathMovement>();
        newPath.GetComponent<pathMovement>().pathExplorerCharacter = newPathExplorer;
        newPath.GetComponent<pathMovement>().moveSpeed = RandomFromDistribution.RandomRangeNormalDistribution(baseSpeed-baseSpeedAway, baseSpeed+baseSpeedAway, conf_level);
        newPath.transform.parent = newPaths.transform;  
    }

}

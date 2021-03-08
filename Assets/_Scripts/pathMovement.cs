using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovement : MonoBehaviour
{
    public GameObject pathExplorerCharacter;
    public float moveSpeed;
    float timer;
    static Vector3 currentPositionHolder;
    int currentNode;
    static Vector3 startPositionEachUpdate;
    // Start is called before the first frame update
    void Start()
    {
        CheckNode();
    }

    void CheckNode(){
        startPositionEachUpdate = pathExplorerCharacter.transform.position;
        timer = 0;
        currentPositionHolder = gameObject.transform.GetChild(currentNode).transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * moveSpeed;
        if (pathExplorerCharacter.transform.position != currentPositionHolder)
        {
            pathExplorerCharacter.transform.position = Vector3.MoveTowards(startPositionEachUpdate, currentPositionHolder, timer);
        }
        else
        {
            if (currentNode < gameObject.transform.childCount - 1)
            {
                currentNode++;
                CheckNode();
            }
            else
            {
                currentNode = 0;
                CheckNode();
            }
        }
    }
}

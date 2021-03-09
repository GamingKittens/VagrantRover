using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovement : MonoBehaviour
{
    #region Properties
    public GameObject pathExplorerCharacter;
    public float moveSpeed;
    private float timer;
    private Vector3 currentPositionHolder;
    private int currentNode;
    private Vector3 startPositionEachUpdate;
    #endregion Properties

    #region Initialization
    void Start ()
    {
        CheckNode();
    }
    #endregion Initialization

    #region Methods
    void CheckNode ()
    {
        startPositionEachUpdate = pathExplorerCharacter.transform.position;
        timer = 0;
        currentPositionHolder = gameObject.transform.GetChild(currentNode).transform.position;
    }

    void Update ()
    {
        timer += Time.deltaTime * moveSpeed;
        if (pathExplorerCharacter.transform.position != currentPositionHolder)
        {
            pathExplorerCharacter.transform.position = Vector3.MoveTowards(startPositionEachUpdate, currentPositionHolder, timer);
        }
        else
        {
            currentNode = ++currentNode % gameObject.transform.childCount;
            CheckNode();
        }
    }
    #endregion Methods
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [Tooltip("Quest steps go here :]")]
    public QuestStep[] steps;

    [Header("Misc")]
    public Transform radarRotator; // Move to player later

    [NaughtyAttributes.ReadOnly]
    public int index = 0;

    [NaughtyAttributes.ReadOnly]
    public bool[] completed;

    void Start()
    {
        if (steps.Length==0)
        {
            Debug.LogWarning("Quests cannot be empty, removing quest.");
            Destroy(this);
        }

        InitiateObjectives();
        InitiateMarks();
    }

    void Update()
    {
        Radar();
    }

    void InitiateObjectives()
    {
        for (int i = 0; i < steps.Length; i++)
            for (int j = 0; j < steps[i].objectives.Length; j++)
                steps[i].objectives[j].Initiate(this, i, j);
    }

    void InitiateMarks ()
    {
        completed = new bool[steps[index].objectives.Length];
    }

    public void CompleteSubStep(int _step, int _substep = 0)
    {
        if (_step!=index)
        {
            Debug.Log("Attempted to complete wrong step. Provided: " + _step + ", " + _substep+". Actual: "+index);
            return;
        }

        completed[_substep] = true;
        updateCooldown = 0;

        if (TestForStepCompletion())
            NextStep();
    }

    public bool TestForStepCompletion()
    {
        foreach (bool b in completed)
            if (b == false)
                return false;

        return true;
    }

    public void NextStep()
    {
        foreach (GameObject go in steps[index].enableOnComplete)
            go.SetActive(true);
        foreach (GameObject go in steps[index].disableOnComplete)
            go.SetActive(false);
        index++;
        
        if (index < steps.Length)
        {
            InitiateMarks();
        }
    }

    private Vector3 nearestObjective;
    private float updateCooldown = 0.1f;
    private float updateDelay = 1f;
    void Radar ()
    {
        updateCooldown-=Time.deltaTime;
        if (updateCooldown <= 0)
        {
            nearestObjective = GetNearestObjective(radarRotator.position);
            updateCooldown += updateDelay;
        }

        nearestObjective.y = radarRotator.position.y;
        radarRotator.rotation = Quaternion.LookRotation(nearestObjective - radarRotator.position, Vector3.up);
    }

    public Vector3 GetNearestObjective (Vector3 _pos)
    {
        if (steps[index].objectives.Length==0)
            return steps[index].objectives[0].transform.position;

        float _dist = Mathf.Infinity;
        int _i = 0;
        for (int i = 0; i< steps[index].objectives.Length; i++)
        {
            if (completed[i])
                continue;
            float _diff = Vector3.Distance(_pos, steps[index].objectives[i].transform.position);
            if (_diff < _dist)
            {
                _dist = _diff;
                _i = i;
            }
        }
        return steps[index].objectives[_i].transform.position;
    }
}

[System.Serializable]
public class QuestStep
{
    public Interactable[] objectives;
    public GameObject[] enableOnComplete;
    public GameObject[] disableOnComplete;
}
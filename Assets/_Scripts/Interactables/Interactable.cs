using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool active = false;

    [NaughtyAttributes.ReadOnly]
    public Quest owner;
    [NaughtyAttributes.ReadOnly]
    public int step;
    [NaughtyAttributes.ReadOnly]
    public int substep;

    public void Initiate (Quest _quest, int _step, int _substep)
    {
        owner = _quest;
        step = _step;
        substep = _substep;
    }    

    [NaughtyAttributes.Button]
    public void Interact ()
    {
        if (!active)
            return;
        InteractOverride();

        /*
        foreach (GameObject go in enable)
            go.SetActive(true);
        foreach (GameObject go in disable)
            go.SetActive(false);
        */
    }

    protected void CheckForQuest()
    {
        Debug.Log("Objective claims to be completed\n" + step + ", " + substep);
        if (owner)
            owner.CompleteSubStep(step, substep);
    }

    protected virtual void InteractOverride ()
    {
        CheckForQuest();
    }
}
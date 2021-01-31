using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool active = false;
    // reference to quest

    public void Initiate ()
    {
        // Set questing reference
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
        // if quest, alert
    }

    protected virtual void InteractOverride ()
    {
        CheckForQuest();
    }
}
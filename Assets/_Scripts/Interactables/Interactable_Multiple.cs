using UnityEngine;

public class Interactable_Multiple : Interactable
{
    [Header("References")]
    public Interactable[] interactables;

    [Tooltip("Set to -1 to disbale")]
    public int maxClicks = -1;

    protected override void InteractOverride ()
    {
        foreach (Interactable _int in interactables)
            _int.Interact();

        if (maxClicks > 0)
            if (--maxClicks == 0)
                Destroy(this);
    }
}
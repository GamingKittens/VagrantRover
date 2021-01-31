using UnityEngine;

public class Interactable_LoadLevel : Interactable
{
    public string levelName;

    protected override void InteractOverride ()
    {
        SceneTransitioner.LoadScene(levelName);
    }
}
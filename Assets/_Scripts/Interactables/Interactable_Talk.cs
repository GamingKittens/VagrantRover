using UnityEngine;

public class Interactable_Talk : Interactable
{
    [Header("References")]
    public AudioSource audioSource;

    [Header("Content")]
    public Dialogue[] dialogue;

    [Header("Options")]
    [Tooltip("Randomly cycles through text")]
    public bool randomize;

    protected int index = -1;

    protected override void InteractOverride ()
    {
        if (randomize)
            PlayRandom();
        else
            PlayNext();
    }

    private void PlayRandom ()
    {
        audioSource?.Stop();

        int i = Random.Range(0, dialogue.Length);

        if (dialogue[i].text != "")
            DisplayText(dialogue[index].text);
        if (dialogue[i].clip != null)
            audioSource?.PlayOneShot(dialogue[index].clip);
    }

    private void PlayNext ()
    {
        if (index >= dialogue.Length - 1)
        {
            CheckForQuest();
            return;
        }
        else
            index++;

        audioSource?.Stop();

        if (dialogue[index].text != "")
            DisplayText(dialogue[index].text);
        if (dialogue[index].clip != null)
            audioSource?.PlayOneShot(dialogue[index].clip);
    }

    private void DisplayText(string _txt)
    {
        UIController.DisplaySubtitles(_txt);
    }
}

[System.Serializable]
public class Dialogue
{
    public string text;
    public AudioClip clip;
}
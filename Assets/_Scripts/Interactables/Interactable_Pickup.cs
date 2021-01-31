using UnityEngine;

public class Interactable_Pickup : Interactable
{
    [Tooltip("Ambient audio of pickup")]
    public AudioSource audioSorce;
    public float duration = 1.5f;

    private Vector3 initialPosition;
    private Vector3 initialScale;
    private float initialVolume;
    private float animationStart = float.NegativeInfinity;

    void Start ()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
        if (audioSorce)
            initialVolume = audioSorce.volume;
    }

    void Update ()
    {
        if (Time.time < animationStart + duration)
        {
            float _fac =  1 - ((Time.time - animationStart) / duration);
            transform.localScale = initialScale * _fac;
            if (audioSorce)
                audioSorce.volume = initialVolume * _fac;
        }
        else if (animationStart > 0)
            EndAnimation();
    }

    protected override void InteractOverride ()
    {
        StartAnimation();
    }

    private void StartAnimation ()
    {
        active = false;
        animationStart = Time.time;
    }

    private void EndAnimation ()
    {
        gameObject.SetActive(false);
        Reset();
        CheckForQuest();
    }

    private void Reset ()
    {
        animationStart = float.NegativeInfinity;
        transform.position = initialPosition;
        transform.localScale = initialScale;
    }
}
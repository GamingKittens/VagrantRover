using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Animation : Interactable
{
    public Transform target;
    public float duration;
    public Vector3 velocity;
    private bool animating;
    private float startTime = -1;

    protected override void InteractOverride()
    {
        StartAnimation();
    }

    private void Start()
    {
        if (!target)
        {
            Debug.LogWarning("No target set, unintended use case");
            target = transform;
        }
    }

    private void Update()
    {
        if (animating)
        {
            if (Time.time < startTime + duration)
                target.position += velocity * Time.deltaTime;
            else
                animating = false;
        }
    }

    protected void StartAnimation()
    {
        animating = true;
        startTime = Time.time;
    }
}

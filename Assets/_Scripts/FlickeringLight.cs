using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public int updatesPerSecond;
    public float multiplier;
    private float baseIntensity;
    private int timePerUpdate;
	private float timer;
    private Light mainLight;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        mainLight = GetComponent<Light>();
        baseIntensity = mainLight.intensity;
        timePerUpdate = (int)(Mathf.Floor(60.0f/updatesPerSecond));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( timer == timePerUpdate){
            int positive = 1;
            if (Random.value < 0.5){
                positive = -1;
            }
            mainLight.intensity = multiplier * Mathf.PerlinNoise(Time.time * 1.0f, 0.0f);
            // mainLight.intensity = mainLight.intensity + 100*Random.value*positive;

            Debug.Log(mainLight.intensity);
        timer = -1;
        }
    timer++;
    }
}

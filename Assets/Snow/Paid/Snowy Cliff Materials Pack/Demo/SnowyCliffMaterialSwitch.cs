using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowyCliffMaterialSwitch : MonoBehaviour
{
    // Reference to the sphere object.
   
    // The material that is to be selected.
    public Material Material1;
    public Material Material2;
    public Material Material3;
 

    
    
    [SerializeField]
    private Renderer planeRenderer;
    [SerializeField]
    private Renderer plane2Renderer;
    [SerializeField]
    private Renderer plane3Renderer;

    
    public void Texture1() {
        planeRenderer.sharedMaterial = Material1;
        plane2Renderer.sharedMaterial = Material1;
        plane3Renderer.sharedMaterial = Material1;
    }
    public void Texture2() {
        planeRenderer.sharedMaterial = Material2;
        plane2Renderer.sharedMaterial = Material2;
        plane3Renderer.sharedMaterial = Material2;
    }
    public void Texture3() {
        planeRenderer.sharedMaterial = Material3;
        plane2Renderer.sharedMaterial = Material3;
        plane3Renderer.sharedMaterial = Material3;
    }
    

    
    
    public void LowQuality() {
        QualitySettings.SetQualityLevel(0, true);
        QualitySettings.masterTextureLimit = 3;
    }
    public void MedQuality() {
        QualitySettings.SetQualityLevel(2, true);
        QualitySettings.masterTextureLimit = 2;
    }
    public void HighQuality() {
        QualitySettings.SetQualityLevel(4, true);
        QualitySettings.masterTextureLimit = 1;
    }
    public void UltraQuality() {
        QualitySettings.SetQualityLevel(5, true);
        QualitySettings.masterTextureLimit = 0;
    }
   
}


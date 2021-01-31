using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject creditsPanel;

    void Start()
    {
        if (!menuPanel || !creditsPanel)
            Debug.LogWarning("Not all panels set in editor!");

        menuPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void StartButton ()
    {
        SceneTransitioner.LoadScene("JunkYard");
    }

    public void CreditsButton ()
    {
        menuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void CloseCreditsButton ()
    {
        menuPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void ExitButton ()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
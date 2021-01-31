using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Transform player;

    public GameObject pauseButton;
    public GameObject pausePanel;
    public GameObject subsPanel;
    public Text subsText;

    public Slider musicSlider;
    public Slider dialogueSlider;
    public Slider environmentSlider;
    public Slider sfxSlider;

    public AudioMixer audioMixer;

    private float initialTimeScale = 1;
    private float removeSubs = -1;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogWarning("Multiple UIControllers found in the same scene!");
            Destroy(this);
        }
        else
            instance = this;
    }

    public void Start()
    {
        subsPanel.SetActive(false);
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        UpdateSliders();

        if (!player)
            Debug.LogWarning("Scene does not have player reference set");
    }

    private void Update()
    {
        if (Time.time >= removeSubs)
        {
            HideSubtitles();
        }    
    }

    public static void DisplaySubtitles(string _txt, float duration = 6f)
    {
        if (!instance)
        {
            Debug.LogError("No UIController present in scene!");
            return;
        }

        instance.subsText.text = _txt;
        instance.subsPanel.SetActive(true);
        instance.removeSubs = Time.time + duration;
    }

    void HideSubtitles ()
    {
        subsPanel.SetActive(false);
        subsText.text = "";
        removeSubs = -1;
    }

    private void UpdateSliders()
    {
        audioMixer.GetFloat("MusicVol", out float musicVol);
        audioMixer.GetFloat("DialogueVol", out float dialogueVol);
        audioMixer.GetFloat("EnvironmentVol", out float environmentVol);
        audioMixer.GetFloat("SFXVol", out float sfxVol);

        musicSlider.value = Mathf.Pow(10, musicVol / 20);
        dialogueSlider.value = Mathf.Pow(10, dialogueVol / 20);
        environmentSlider.value = Mathf.Pow(10, environmentVol / 20);
        sfxSlider.value = Mathf.Pow(10, sfxVol / 20);
    }

    public void ReturnToMenu ()
    {
        SceneTransitioner.LoadScene("Main Menu");

        pauseButton.SetActive(false);
        pausePanel.SetActive(false);

        Time.timeScale = initialTimeScale;
    }

    public void UnstickPlayer ()
    {
        player.position += Vector3.up * 1;
        player.rotation = Quaternion.Euler(Vector3.up * player.rotation.eulerAngles.y);
    }

    public void ReloadLevel ()
    {
        SceneTransitioner.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        pauseButton.SetActive(false);
        pausePanel.SetActive(false);

        Time.timeScale = initialTimeScale;
    }    

    public void PauseButton ()
    {
        initialTimeScale = Time.timeScale;
        Time.timeScale = 0;

        pauseButton.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void UnpauseButton ()
    {
        pauseButton.SetActive(true);
        pausePanel.SetActive(false);

        Time.timeScale = initialTimeScale;
    }

    public void SetMusicLevel ()
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(musicSlider.value) * 20);
    }    

    public void SetDialogueLevel ()
    {
        audioMixer.SetFloat("DialogueVol", Mathf.Log10(dialogueSlider.value) * 20);
    }

    public void SetEnvironmentLevel()
    {
        audioMixer.SetFloat("EnvironmentVol", Mathf.Log10(environmentSlider.value) * 20);
    }

    public void SetSFXLevel()
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(sfxSlider.value) * 20);
    }
}
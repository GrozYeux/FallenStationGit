using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public GameObject CanvasOptions;
    public GameObject panel;
    public GameObject Menu;
    GameObject EventSystem;
    public GameObject ButtonCodex;
    public Dropdown dResolution;
    public Dropdown dQuality;
    public Slider sliderMusique;
    public Slider sliderSFX;

    //public AudioMixer audioMixer;
    public Text textMusique;
    public Text textSound;
    Resolution[] resolutions;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem = GameObject.Find("EventSystem");
        AddResolution();
        dQuality.value = PlayerPrefs.GetInt("Quality", 2);
        dResolution.value = PlayerPrefs.GetInt("Resolution", 21);
        sliderMusique.value = PlayerPrefs.GetFloat("Musique", 0.7f);
        sliderSFX.value = PlayerPrefs.GetFloat("SFX", 0.75f);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            CanvasOptions.SetActive(false);
            Menu.SetActive(true);
            panel.SetActive(true);

            EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(ButtonCodex);
        }
    }

    public void AddResolution()
    {
        resolutions = Screen.resolutions;
        dResolution.ClearOptions();
        List<string> options = new List<string>();
        int currentResolution = 0;
        for (int i = 0; i <resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }
        dResolution.AddOptions(options);
        dResolution.value = currentResolution;
        dResolution.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
        //PlayerPrefs.SetInt("Resolution", resolutionIndex);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        //PlayerPrefs.SetInt("Quality", qualityIndex);/
    }

    public void SetMusique(float volume)
    {
        float mixerValue = Mathf.Lerp(-80, 20, volume);
        SoundManager.Instance.ChangeMusicVolume(mixerValue);
        textMusique.text = "Musique : " + (volume * 100).ToString("00") + "%";
    }

    public void SetSoundEffect(float volume)
    {
        float mixerValue = Mathf.Lerp(-80, 20, volume);
        SoundManager.Instance.ChangeEffectVolume(mixerValue);
        textSound.text = "Effets sonores :" + (volume * 100).ToString("00") + "%";
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("Musique", sliderMusique.value);
        PlayerPrefs.SetFloat("SFX", sliderMusique.value);
        PlayerPrefs.SetInt("Quality", dQuality.value);
        PlayerPrefs.SetInt("Resolution", dResolution.value);

    }

    
}

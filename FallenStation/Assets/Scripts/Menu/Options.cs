using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public GameObject CanvasOptions;
    public GameObject Menu;

    public Dropdown dResolution;
    public AudioSource audioSource;
    //public AudioMixer audioMixer;
    Slider sliderMusique;
    Slider sliderSound;
    Text textMusique;
    Text textSound;
    Resolution[] resolutions;
    // Start is called before the first frame update
    void Start()
    {
        textMusique = GameObject.Find("TextMusique").GetComponent<Text>();
        textSound = GameObject.Find("TextEffetsSonores").GetComponent<Text>();
        AddResolution();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            CanvasOptions.SetActive(false);
            Menu.SetActive(true);
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
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetMusique(float volume)
    {
        audioSource.volume = volume;
        textMusique.text = "Musique : " + (audioSource.volume * 100).ToString("00") + "%";

    }

    public void SetSoundEffect(float volume)
    {
        //audioMixer.SetFloat("volume",volume);
        //textSound.text = "Effets sonores :" + (audioMixer.GetFloat("volume",volume)* 100).ToString("00") + "%";
    }

}

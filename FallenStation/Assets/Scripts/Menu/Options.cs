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
    //public AudioSource soundEffectSource;
    Slider sliderMusique;
    Slider sliderSound;
    Text textMusique;
    Text textSound;

    // Start is called before the first frame update
    void Start()
    {
        sliderMusique = GameObject.Find("SliderMusique").GetComponent<Slider>();
        sliderSound = GameObject.Find("SliderEffetsSonores").GetComponent<Slider>();
        textMusique = GameObject.Find("TextMusique").GetComponent<Text>();
        textSound = GameObject.Find("TextEffetsSonores").GetComponent<Text>();
        SliderChange();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            CanvasOptions.SetActive(false);
            Menu.SetActive(true);
        }
    }

    public void SetResolution()
    {
        switch(dResolution.value)
        {
            case 0:
                Screen.SetResolution(640, 480, true);
                break;
            case 1:
                Screen.SetResolution(1280, 720, true);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, true);
                break;
            default:
                Screen.SetResolution(1920, 1080, true);
                break;

        }
    }

    public void SliderChange()
    {
        audioSource.volume = sliderMusique.value;
        textMusique.text = "Musique : " + (audioSource.volume * 100).ToString("00") + "%";
        //soundEffectSource.volume = sliderSound.value;
        //textSound.text = "Effets sonores :" + (soundEffectSource.volume * 100).ToString("00") + "%";
    }
    
}

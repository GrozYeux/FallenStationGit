using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public string sceneJeu;
    // Start is called before the first frame update
    public static bool load = false;
    public GameObject resumeButton;
    AudioSource sound;
    void Start()
    {
        sound = GameObject.Find("Sound").GetComponent<AudioSource>();
        sound.volume = 1;
        DontDestroyOnLoad(sound);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (SaveSystem.LoadPlayer()!=null)
        {
            resumeButton.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    
    public void PlayGame()
    {
        
        Debug.Log("Chargement");
        SceneManager.LoadScene(sceneJeu);
      
    }

    public void ResumeGame()
    {
        Debug.Log("Chargement");
        load = true;
        SceneManager.LoadScene(sceneJeu);
    }
    
}

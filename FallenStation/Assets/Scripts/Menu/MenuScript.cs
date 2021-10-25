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
    void Start()
    {
        DontDestroyOnLoad(GameObject.Find("Sound"));
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.wasRunning)
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

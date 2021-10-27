using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject Panel;
    GameObject EventSystem;
    public GameObject ButtonCodex;
    public static bool isPaused = false;
    public static bool wasRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem = GameObject.Find("EventSystem");
       
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
            


        }
    }

    void Pause()
    {
        Panel.SetActive(true);
        EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(ButtonCodex);
        Time.timeScale = 0f;
        isPaused = true;
    }

    void Resume()
    {
        Panel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void LoadMenu()
    {
        //Save the data of the players before leaving
        Resume();
        SaveSystem.SavePlayer(GameObject.Find("Player").GetComponent<PlayerMovementScript>());
        wasRunning = true;
        SceneManager.LoadScene("Menu");

    }

    public void LoadOptions()
    {

    }

    public void LoadAide()
    {

    }

    public void LoadCodex()
    {

    }
}

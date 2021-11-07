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
    // Start is called before the first frame update
    void Start()
    {
        Resume();
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
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        isPaused = true;
    }

    void Resume()
    {
        Panel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    public void LoadMenu()
    {
        //Save the data of the players before leaving
        SaveSystem.SavePlayer(GameObject.Find("Player").GetComponent<PlayerMovementScript>());
        MenuScript.currentscene = SceneManager.GetActiveScene().name;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Menu");


    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject Panel;
    [SerializeField]
    GameObject EventSystem;
    public GameObject ButtonCodex;
    public static bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        Resume();
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
        GameManager.Instance.GetPlayer().GetComponentInChildren<Gun>().canFire = false;
        GameManager.Instance.GetPlayer().GetComponentInChildren<MouseLook>().canLookAround = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        isPaused = true;
    }

    void Resume()
    {
        Panel.SetActive(false);
        GameManager.Instance.GetPlayer().GetComponentInChildren<Gun>().canFire = true;
        GameManager.Instance.GetPlayer().GetComponentInChildren<MouseLook>().canLookAround = true;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    public void LoadMenu()
    {
        GameObject player = GameManager.Instance.GetPlayer();
        //Save the data of the players before leaving
        SaveSystem.SavePlayer(player.GetComponent<PlayerMovementScript>(),player.GetComponent<TimeWarp>());
        if (Collectables.Instance != null)
        {
            SaveSystem.SaveObject(Collectables.Instance);
        }
        MenuScript.currentscene = SceneManager.GetActiveScene().name;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Menu");


    }

}

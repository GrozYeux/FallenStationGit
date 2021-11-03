using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINote : MonoBehaviour
{
    GameObject canvasNote;
    public static bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        canvasNote = GameObject.Find("CanvasNote");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isPaused)
            {
                Resume();
            }

        }
    }

    public static void Pause()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    void Resume()
    {
        canvasNote.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }
}

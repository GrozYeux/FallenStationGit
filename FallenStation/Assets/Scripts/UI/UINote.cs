using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINote : AbstractSingleton<UINote>
{ 
    public static GameObject canvasNote;
    public static bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        canvasNote = GameObject.Find("note");
        canvasNote.SetActive(false);

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
        Scrollbar scroll = canvasNote.GetComponentInChildren<Scrollbar>();
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
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

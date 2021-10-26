using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodexManager : MonoBehaviour
{
    public GameObject content;
    private Button button;
    private Button[] buttons;

    
    public GameObject panel;
    public GameObject Menu;
    GameObject CanvasCodex;
    GameObject EventSystem;
    public GameObject ButtonCodex;
    // Start is called before the first frame update
    void Start()
    {
        buttons = content.GetComponentsInChildren<Button>();
        CanvasCodex = GameObject.Find("Codex");
        EventSystem = GameObject.Find("EventSystem");

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) buttons[0].interactable = true;
        if (Input.GetKeyDown(KeyCode.Alpha2)) buttons[1].interactable = true;
        if (Input.GetKeyDown(KeyCode.Alpha3)) buttons[2].interactable = true;
        if (Input.GetKeyDown(KeyCode.Alpha4)) buttons[3].interactable = true;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasCodex.SetActive(false);
            Menu.SetActive(true);
            panel.SetActive(true);

            EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(ButtonCodex);
        }
    }
}

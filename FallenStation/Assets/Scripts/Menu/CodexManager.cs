using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodexManager : MonoBehaviour
{
    public GameObject content;
    private Button button;
    private Button[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        buttons = content.GetComponentsInChildren<Button>();

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) buttons[0].interactable = true;
        if (Input.GetKeyDown(KeyCode.Alpha2)) buttons[1].interactable = true;
        if (Input.GetKeyDown(KeyCode.Alpha3)) buttons[2].interactable = true;
        if (Input.GetKeyDown(KeyCode.Alpha4)) buttons[3].interactable = true;
    }
}

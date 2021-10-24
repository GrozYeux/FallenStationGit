using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : MonoBehaviour
{
    public GameObject CanvasHelp;
    public GameObject panel;
    public GameObject Menu;
    GameObject EventSystem;
    GameObject ButtonCodex;
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
            CanvasHelp.SetActive(false);
            Menu.SetActive(true);
            panel.SetActive(true);
            ButtonCodex = GameObject.Find("ButtonCodex");

            EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(ButtonCodex);
        }
    }
}

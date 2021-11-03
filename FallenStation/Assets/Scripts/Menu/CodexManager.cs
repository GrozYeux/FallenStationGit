using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CodexManager : MonoBehaviour
{
    public GameObject content;
    private Button[] buttons;
    public string[] notes;

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            CanvasCodex.SetActive(false);
            Menu.SetActive(true);
            panel.SetActive(true);

            EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(ButtonCodex);
        }

        
    }
   
    public void instanciateCodex()
    {
        TextManager tm = GameObject.Find("TextManager").GetComponent<TextManager>();
        buttons = content.GetComponentsInChildren<Button>();
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if(SaveSystem.LoadCodex() !=null){
                //Récuperer la sauvegarde du codex si elle existe 
                CodexData data = SaveSystem.LoadCodex();
                notes = data.notes;
            }
            
        }
        else 
        {
            Data AllData = Sas.Load();
            if (AllData.codexData != null)
            {
                notes = AllData.codexData.notes;
            }
            else
            {
                notes = Collectables.Instance.ArrayNotes();
            }
            
        }
        // bloucle for sur le nombre de codes dans la scéne 
        for (int i = 0; i < notes.Length; i++)
        {

            //création du boutton dans le codex 
            Text[] info = buttons[i].GetComponentsInChildren<Text>();
            print(info);
            info[0].text = "#";
            info[1].text = "" + (i + 1);
            info[2].text = notes[i];
            buttons[i].interactable = true;
            buttons[i].onClick.AddListener(() => tm.DisplayNote(info[2].text));

        }
    }
}

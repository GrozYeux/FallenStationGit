using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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
      
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            //Récuperer la sauvegarde du codex si elle existe 
        }
        else
        {
            // bloucle for sur le nombre de codes dans la scéne 
            TextManager tm = GameObject.Find("TextManager").GetComponent<TextManager>();
            Collectables col = GameObject.Find("Player").GetComponent<Collectables>();
            GameObject content = GameObject.Find("Content");
            Button[] codexEntry = content.GetComponentsInChildren<Button>();
            HashSet<string> notes = new HashSet<string>();
            notes = col.getNotes();
            for(int i = 0; i < notes.Count; i++)
            {

                //création du boutton dans le codex 
                Text[] info = codexEntry[i].GetComponentsInChildren<Text>();
                print(info);
                info[0].text = "#";
                info[1].text = "" + (i+1);
                info[2].text = notes.Skip(i).First();
                codexEntry[i].interactable = true;
                codexEntry[i].onClick.AddListener(() => tm.DisplayNote(info[2]));
                
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;



public class UI_InputWindow : MonoBehaviour
{
    GameObject EventSystem;
    public Button okBtn;
    private Button cancelBtn;
    private Button indiceBtn;
    private Text title;
    private Text description;
    private InputField inputField_1;
    private InputField inputField_2;
    private InputField inputField_3;
    private InputField inputField_4;
    private String code;
    private int i;
    // Use this for initialization
    void Awake()
    {
        i = 0;
        EventSystem = GameObject.Find("EventSystem");
        okBtn = transform.Find("confirmBtn").GetComponent<Button>();
        cancelBtn = transform.Find("cancelBtn").GetComponent<Button>();
        indiceBtn = transform.Find("indiceBtn").GetComponent<Button>();
        title = transform.Find("Title").GetComponent<Text>();
        description = transform.Find("Description").GetComponent<Text>();;
        inputField_1 = transform.Find("InputField_1").GetComponent<InputField>();
        inputField_2 = transform.Find("InputField_2").GetComponent<InputField>();
        inputField_3 = transform.Find("InputField_3").GetComponent<InputField>();
        inputField_4 = transform.Find("InputField_4").GetComponent<InputField>();
        Hide();
    }

    void Update()
    {
        code = inputField_1.text + inputField_2.text + inputField_3.text + inputField_4.text;
    }
    public void Show(string titleString, string descriptionString, Action<string> onConfirm, UnityEngine.UI.InputField.ContentType type, String[] indices)
    {

        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        i = 0;
        gameObject.SetActive(true);
        EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(inputField_1.gameObject);
        inputField_1.contentType = type;
        inputField_2.contentType = type;
        inputField_3.contentType = type;
        inputField_4.contentType = type;
        if (type == InputField.ContentType.IntegerNumber)
        {
            inputField_1.placeholder.GetComponent<Text>().text = "0";
            inputField_2.placeholder.GetComponent<Text>().text = "0";
            inputField_3.placeholder.GetComponent<Text>().text = "0";
            inputField_4.placeholder.GetComponent<Text>().text = "0";
        }
        title.text = titleString;
        description.text = descriptionString;
        okBtn.onClick.AddListener(delegate
        {
            indiceBtn.onClick.RemoveAllListeners();
            okBtn.onClick.RemoveAllListeners();
            onConfirm(code);
            DeleteText();
            
        });

        cancelBtn.onClick.AddListener(delegate
       {
           Hide();
       });
        indiceBtn.onClick.AddListener(delegate
        {
            DeleteText();
            print(i);
            showIndice(indices[i]);
            i = (i+1) % indices.Length;

        });
    }

    public void Hide()
    {
        DeleteText();
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void DeleteText()
    {
        inputField_1.text = "";
        inputField_2.text = "";
        inputField_3.text = "";
        inputField_4.text = "";
    }


    public void Indice(string titleString, string descriptionString, string indice)
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        title.text = titleString;
        description.text = descriptionString;
        showIndice(indice);
    }


    public void showIndice(string indice)
    {
        inputField_1.placeholder.GetComponent<Text>().text = indice.Substring(0, 1);
        inputField_2.placeholder.GetComponent<Text>().text = indice.Substring(1, 1);
        inputField_3.placeholder.GetComponent<Text>().text = indice.Substring(2, 1);
        inputField_4.placeholder.GetComponent<Text>().text = indice.Substring(3, 1);
    }

    public void End(string titleString, string descriptionString, Action onConfirm)
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(okBtn.gameObject);
        title.text = titleString;
        description.text = descriptionString;
        indiceBtn.gameObject.SetActive(false);
        inputField_1.gameObject.SetActive(false);
        inputField_2.gameObject.SetActive(false);
        inputField_3.gameObject.SetActive(false);
        inputField_4.gameObject.SetActive(false);

        okBtn.onClick.AddListener(delegate
        {
            onConfirm();
            Hide();

        });

        cancelBtn.onClick.AddListener(delegate
        {
            Hide();
        });
    }
}
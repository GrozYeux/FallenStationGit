using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;



public class UI_InputWindow : MonoBehaviour
{
    GameObject EventSystem;
    private Button okBtn;
    private Button cancelBtn;
    private Text title;
    private Text description;
    private InputField inputField_1;
    private InputField inputField_2;
    private InputField inputField_3;
    private InputField inputField_4;
    private String code;
    // Use this for initialization
    void Awake()
    {
        EventSystem = GameObject.Find("EventSystem");
        okBtn = transform.Find("confirmBtn").GetComponent<Button>();
        cancelBtn = transform.Find("cancelBtn").GetComponent<Button>();
        title = transform.Find("Title").GetComponent<Text>();
        description = transform.Find("Description").GetComponent<Text>();
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
    public void Show(string titleString, string descriptionString, Action<string> onConfirm, UnityEngine.UI.InputField.ContentType type)
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
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
            onConfirm(code);
            inputField_1.text = "";
            inputField_2.text = "";
            inputField_3.text = "";
            inputField_4.text = "";
        });

        cancelBtn.onClick.AddListener(delegate
       {
           Hide();
       });
    }

    public void Hide()
    {
        inputField_1.text = "";
        inputField_2.text = "";
        inputField_3.text = "";
        inputField_4.text = "";
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }


    public void Indice(string titleString, string descriptionString, string indice)
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        title.text = titleString;
        description.text = descriptionString;
        inputField_1.placeholder.GetComponent<Text>().text = indice.Substring(0,1);
        inputField_2.placeholder.GetComponent<Text>().text = indice.Substring(1, 1);
        inputField_3.placeholder.GetComponent<Text>().text = indice.Substring(2, 1);
        inputField_4.placeholder.GetComponent<Text>().text = indice.Substring(3, 1);
    }
}
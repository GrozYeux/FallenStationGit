using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class UI_InputWindow : MonoBehaviour
    {
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
            okBtn = transform.Find("confirmBtn").GetComponent<Button>();
            cancelBtn = transform.Find("cancelBtn").GetComponent<Button>();
            title = transform.Find("Title").GetComponent<Text>();
            description = transform.Find("Description").GetComponent<Text>();
            inputField_1 = transform.Find("inputField_1").GetComponent<InputField>();
            inputField_2 = transform.Find("inputField_2").GetComponent<InputField>();
            inputField_3 = transform.Find("inputField_3").GetComponent<InputField>();
            inputField_4 = transform.Find("inputField_4").GetComponent<InputField>();
        Hide();
        }

    void Update()
    {
        code = inputField_1.text + inputField_2.text + inputField_3.text + inputField_4.text;
    }
    public void Show(string descriptionString, Action onCancel, Action<string> onConfirm)
    {
        gameObject.SetActive(true);
        description.text = descriptionString;
        okBtn.onClick.AddListener(delegate {
            onConfirm(code);
        });

        cancelBtn.onClick.AddListener (delegate
        {
            Hide();
            onCancel();
        });
        }

        public void Hide()
        {

            gameObject.SetActive(false);
        }
    }

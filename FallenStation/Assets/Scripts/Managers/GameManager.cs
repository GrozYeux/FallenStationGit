using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject panelMort;
    [SerializeField]
    private GameObject EventSystem;

    private void Awake()
    {
        panelMort.SetActive(false);
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        panelMort.SetActive(true);
        GameObject.Find("UI_Camera").GetComponent<Kino.AnalogGlitch>().scanLineJitter = 0.9f;
        GameObject.Find("UI_Camera").GetComponent<Kino.AnalogGlitch>().colorDrift = 0.08f;
        Button continueBtn = panelMort.GetComponentsInChildren<Button>()[0];
        Button quitBtn = panelMort.GetComponentsInChildren<Button>()[1];
        EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(continueBtn.gameObject);
        Debug.Log(EventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject);
        
        

        continueBtn.onClick.AddListener(delegate
        {
            
            Destroy(player);
            MenuScript.load = true;
            if (MenuScript.currentscene == null)
            {
                MenuScript.currentscene = SceneManager.GetActiveScene().name;
            }
            SceneManager.LoadScene(MenuScript.currentscene);
        });
        quitBtn.onClick.AddListener(delegate
        {
            
            Destroy(player);
            MenuScript.currentscene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("Menu");
        });
        modifyPanel(0.9f);
        
    }

    protected void modifyPanel(float newAlpha)
    {
        Image image = panelMort.GetComponentsInChildren<Image>()[0];
        var tempColor = image.color;
        tempColor.a = newAlpha;
        image.color = tempColor;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

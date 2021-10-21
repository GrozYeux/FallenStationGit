using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    
    void PlayGame()
    {
        Debug.Log("Chargement");
        SceneManager.LoadScene("testScene");
    }
    
}

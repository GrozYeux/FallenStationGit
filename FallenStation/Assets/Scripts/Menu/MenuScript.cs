using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public static string currentscene;
    public string sceneJeu;
    // Start is called before the first frame update
    public static bool load = false;
    public GameObject resumeButton;
    [SerializeField]
    AudioClip music;

    private void Awake()
    {
        GameObject potentialPlayer = GameObject.Find("Player");
        if (potentialPlayer != null)
        {
            Destroy(potentialPlayer);
        }
    }
    void Start()
    {
        SoundManager.Instance.PlayMusic(music);
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveSystem.LoadPlayer() != null || Sas.Load()!= null)
        {
            resumeButton.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    
    public void PlayGame()
    {
        SaveSystem.DeletePlayer();
        SaveSystem.DeleteCodex();
        SaveSystem.DeleteObject();
        Sas.Delete();
        
        SceneManager.LoadScene(sceneJeu);

        if (Collectables.Instance != null)
        {
            Collectables.Instance.DeleteObject();
            Collectables.Instance.DeleteNote();
        }

    }

    public void ResumeGame()
    {
        Debug.Log("Chargement");
        load = true;
        print(currentscene);
        if(currentscene == null)
        {
            currentscene = sceneJeu;
        }
        SceneManager.LoadScene(currentscene);
    }
    
}

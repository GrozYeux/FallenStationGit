using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private string levelName;
    
    public void GotoLevel()
    {
        if (levelName == null)
        {
            Debug.Log("Going to level");
            SceneManager.LoadScene(levelName);
        }
    }


    private void OnEnable()
    {
        GotoLevel();
    }
}

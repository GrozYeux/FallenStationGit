using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Sas : MonoBehaviour
{
   

    public static Data Load()
    {
        string path = Application.persistentDataPath + "/sas.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Data data = formatter.Deserialize(stream) as Data;
            stream.Close();
            return data;
        }
        else
        {

            return null;
        }
    }

    public static void Save (PlayerMovementScript player,Collectables collectable)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/sas.txt";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        Data data = new Data(player, collectable);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void Delete()
    {
        string path = Application.persistentDataPath + "/sas.txt";
        File.Delete(path);
    }

    public static void nextLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        int i = scene.buildIndex;
        MenuScript.currentscene = "Level" + (i+1);
        SaveSystem.DeleteCodex();
        SaveSystem.DeletePlayer();
        SaveSystem.DeleteObject();
        Destroy(GameObject.Find("Player"));
        SceneManager.LoadScene("Level" + (i + 1));
            
        
    }
}

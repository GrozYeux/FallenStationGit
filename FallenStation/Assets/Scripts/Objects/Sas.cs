using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Sas 
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

    public static void nextLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        Scene[] scenes = SceneManager.GetAllScenes();
        for(int i =0; i < scenes.Length; i++)
        {
            if(scenes[i] == scene && scenes[i].name != "LevelFinal")
            {
                SceneManager.LoadScene(scenes[i + 1].name);
            }
        }
    }
}

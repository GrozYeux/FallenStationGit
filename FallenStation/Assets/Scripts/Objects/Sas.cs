using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

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
}

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayer(PlayerMovementScript player, TimeWarp tw)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player,tw);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveCodex(Collectables codex)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/codex.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        CodexData data = new CodexData(codex);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveObject(Collectables collectable)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/object.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        ObjectData data = new ObjectData(collectable);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer ()
    {
        string path = Application.persistentDataPath + "/player.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
       
            return null;
        }
    }

    public static CodexData LoadCodex()
    {
        string path = Application.persistentDataPath + "/codex.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CodexData data = formatter.Deserialize(stream) as CodexData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }

    public static ObjectData LoadObject()
    {
        string path = Application.persistentDataPath + "/object.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ObjectData data = formatter.Deserialize(stream) as ObjectData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }


    public static void DeletePlayer()
    {
        string path = Application.persistentDataPath + "/player.txt";
        if (File.Exists(path))
            File.Delete(path);
    }

    public static void DeleteCodex()
    {
        string path = Application.persistentDataPath + "/codex.txt";
        if (File.Exists(path))
            File.Delete(path);
    }

    public static void DeleteObject()
    {
        string path = Application.persistentDataPath + "/object.txt";
        if (File.Exists(path))
            File.Delete(path);
    }

}

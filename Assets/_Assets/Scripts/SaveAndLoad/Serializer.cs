using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Serializer
{
    public static void SaveGame(InventoryView inventoryView)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Saves.kevin";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(inventoryView);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/Saves.kevin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;

            stream.Close();
            Debug.Log("Data Loaded");
            return data;
        }
        else
        {
            Debug.Log("Save File not found!");
            return null;
        }
    }
}

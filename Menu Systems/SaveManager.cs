using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager 
{
    //Takes PlayerData class and converts into binary data to be stored on the users device.
    public static void Save(PlayerData luperSaveData)
    {
        string saveDataPath = Application.persistentDataPath + "/player.persistence";
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream dataStream = File.Create(saveDataPath))
        {
            binaryFormatter.Serialize(dataStream, luperSaveData);
        }
    }

    //Loads PlayerData class from binary and converts into recognisable data.
    public static PlayerData Load()
    {
        string loadDataPath = Application.persistentDataPath + "/player.persistence";

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        if (!File.Exists(loadDataPath))
        {
            Debug.LogError("SaveManager: Save Data could not be found at the location: " + loadDataPath);
            return null;
        }

        using(FileStream dataStream = new FileStream(loadDataPath, FileMode.Open))
        {
            PlayerData gameData = binaryFormatter.Deserialize(dataStream) as PlayerData;
            return gameData;
        }

    }
}

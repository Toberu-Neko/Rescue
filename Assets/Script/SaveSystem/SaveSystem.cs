using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerStates playerStates, int saveSlot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PlayerData" + saveSlot + ".neko";

        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerDataTransfer data = new PlayerDataTransfer(playerStates);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerDataTransfer LoadPlayer(int saveSlot)
    {
        string path = Application.persistentDataPath + "/PlayerData" + saveSlot + ".neko";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerDataTransfer data = formatter.Deserialize(stream) as PlayerDataTransfer;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}

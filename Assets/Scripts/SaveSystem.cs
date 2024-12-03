using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Static Class so an instance cannot be declared (can only be called upon)
public static class SaveSystem
{
    // Saves player data into a file on the OS in binary formatting
    public static void SavePlayer (PlayerData player)
    {
        Debug.Log("Called Save PLayer");  // REMOVE

        // Binary formatter for translating data into binary
        BinaryFormatter formatter = new BinaryFormatter();

        // Saving into an area dependant on OS with a filename & stream to save data with
        string path = Application.persistentDataPath + "/player.pookie";
        FileStream stream = new FileStream(path, FileMode.Create);

        // Write data to file through stream
        formatter.Serialize(stream, player);
        stream.Close();

        Debug.Log("Completed Save PLayer");  // REMOVE
    }

    // Loads player data from a file on the OS by translating from binary
    public static PlayerData LoadPlayer()
    {
        Debug.Log("Called Load PLayer");  // REMOVE

        // Check if file exists in path specified before accessing it
        string path = Application.persistentDataPath + "/player.pookie";
        if (File.Exists(path))
        {
            // Load binary formatter and stream to read file
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            // Read from stream and translate from binary
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            Debug.Log("Completed Load PLayer");  // REMOVE

            return data;
        } 
        else
        {
            Debug.Log("Failed Load PLayer");  // REMOVE

            // Show error and return null
            Debug.LogError("File for player data, cannot be found!");
            return null;
        }
    }
}

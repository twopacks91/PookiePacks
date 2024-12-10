using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

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

        // Attempt to save data, and output any exceptions that occur
        try
        {
            // Will automatically close file
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                // Write data to file through stream
                formatter.Serialize(stream, player);
                stream.Flush();
            }
        }
        catch (IOException ex)
        {
            Debug.LogError("Failed to save player data: " + ex.Message);
        }

        Debug.Log("Completed Save PLayer");  // REMOVE
    }

    // Loads player data from a file on the OS by translating from binary
    public static PlayerData LoadPlayer()
    {
        Debug.Log("Called Load PLayer");  // REMOVE

        // Must be the same path as SavePlayer
        string path = Application.persistentDataPath + "/player.pookie";

        // Attempt to load player data from file and output any exceptions that occur
        try
        {
            // Load binary formatter and stream to read file
            BinaryFormatter formatter = new BinaryFormatter();

            // Read stream into player data class and return it (will close file automatically)
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                // Read from stream and translate from binary
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                Debug.Log("Successfully deserialized player data.");
                return data;
            }           
        }
        catch (SerializationException ex)  // catch specific exception and log error
        {
            Debug.LogError("Failed to deserialize file: " + ex.Message);
            Debug.LogError("The file might be corrupted or incomplete, YIKES.");
            return null;
        }
        catch (Exception ex)
        {
            Debug.LogError("Unexpected error while deserializing: " + ex.Message);
            return null;
        }
    }
}

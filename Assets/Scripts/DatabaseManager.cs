using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{
    // Variables
    private string dbFolder = "Databases";
    private string summonDbName = "SummonTableDraft3.csv";
    private string characterDbName = "CharacterTableDraft1.csv";
    private string newSummonDbPath;
    private string newCharacterDbPath;

    // This should run when app is first run, should attach to loading screen to ensure it happens
    void Awake()
    {
        // Don't copy over if database exists in persistentDataPath (for androids)
        newSummonDbPath = Path.Combine(Application.persistentDataPath, this.summonDbName);
        string originalSummonDbPath = Path.Combine(Application.streamingAssetsPath, dbFolder, this.summonDbName);
        if (!File.Exists(newSummonDbPath))
        {
            CopySummonDbToPersistentPath(originalSummonDbPath, newSummonDbPath);
        }

        // Also check for character database
        newCharacterDbPath = Path.Combine(Application.persistentDataPath, this.characterDbName);
        string originalCharacterDbPath = Path.Combine(Application.streamingAssetsPath, dbFolder, characterDbName);
        if (!File.Exists(newCharacterDbPath))
        {
            CopySummonDbToPersistentPath(originalCharacterDbPath, newCharacterDbPath);
        }
    }

    /// <summary>
    /// Copies a database from streaming assets folder to persistent data path so it can be accessed and written to properly.
    /// </summary>
    /// <param name="originalDbPath"> string path where the database exists in streaming assets folder </param>
    /// <param name="newDbPath"> string path where database will now be stored in persistent data path </param>
    void CopySummonDbToPersistentPath(string originalDbPath, string newDbPath)
    {
        // Android requires UnityWebRequest for streaming assets (this is why ios better)
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(CopyDatabaseAndroid(originalDbPath, newDbPath));
        }
        else
        {
            // Copy directly for windows/mac/linux
            if (File.Exists(originalDbPath))
            {
                File.Copy(originalDbPath, newDbPath, true);
                Debug.Log($"Database copied to persistent data path: {newDbPath}");
            }
            else
            {
                Debug.Log($"Database Manager Error - Database not found in StreamingAssets: {originalDbPath}");
            }
        }
    }

    /// <summary>
    /// Copies database file from streaming assets to persisten data path on Android since streaming assets is in APK and can't be directly accessed.
    /// Uses UnityWebRequest to read file and save it since direct file access is restricted on Android, they just gotta make life harder.
    /// </summary>
    /// <param name="originalDbPath"> string path where the database exists in streaming assets folder </param>
    /// <param name="destinationPath"> string path where database will now be stored in persistent data path </param>
    /// <returns></returns>
    IEnumerator CopyDatabaseAndroid(string originalDbPath, string newDbPath)
    {
        UnityWebRequest www = UnityWebRequest.Get(originalDbPath);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            File.WriteAllBytes(newDbPath, www.downloadHandler.data);
            Debug.Log("Database copied to persistentDataPath (Android): " + newDbPath);
        }
        else
        {
            Debug.LogError("Database Manager Error - Can't copy database on ANDROID: " + www.error);
        }
    }
}

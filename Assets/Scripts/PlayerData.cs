using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

[System.Serializable]  // allows saving to a file
public class PlayerData
{
    // Class Variables
    public static PlayerData instance = null;

    private string mUsername;
    private string mPassword;
    private Character mEquipped;
    private int mMoney;

    private List<Item> mItems;
    private List<Character> mCharacters;
    private List<Mission> mToDoMissions;
    private List<Mission> mDoneMissions;
    private List<Setting> mSettings;


    // *** Initaliser as a singleton - USE THIS
    public static PlayerData GetInstance()
    {
        if (instance == null)
        {
            instance = new PlayerData();
        }
        return instance;
    }


    // *** Constructors - (uncommented for now UNTIL required - UPDATE it is now required - Hamza)
    // Create and Save new player instance - only use in registering, otherwise data will be overwritten
    public PlayerData(string username, string password)
    {
        mUsername = username;
        mPassword = password;
        mMoney = 10;
        mItems = new List<Item>(0);
        mCharacters = new List<Character>(0);
        mDoneMissions = new List<Mission>(0);
        mToDoMissions = new List<Mission>(0);
        mSettings = new List<Setting>(0);

        // Read missions database file to load all missions into To-Do list with zero progress
        string databaseFile = Path.Combine(Application.persistentDataPath, "MissionTable.csv");
        Debug.Log(databaseFile);
        string[] database = File.ReadAllLines(databaseFile);
        foreach (string line in database)
        {
            string[] values = line.Split(',');
            if (int.TryParse(values[0], out int dk))
            {
                int id = int.Parse(values[0]);
                string name = values[1];
                string description = values[2];
                int reward = int.Parse(values[3]);
                int needed = int.Parse(values[4]);
                mToDoMissions.Add(new Mission(id, name, description, reward, needed, 0));
            }
        }

        databaseFile = Path.Combine(Application.persistentDataPath, "SettingsTable.csv");
        Debug.Log(databaseFile);
        database = File.ReadAllLines(databaseFile);
        foreach (string line in database)
        {
            string[] values = line.Split(',');
            if (int.TryParse(values[0], out int dk))
            {
                int id = int.Parse(values[0]);
                string name = values[1];
                string description = values[2];
                mSettings.Add(new Setting(id,name,description,false));
            }
        }
    }
    // Default constructor - loads player using information stored
    private PlayerData()
    {
        this.LoadPlayer();
    }

    // Setters & Getters - better protection of user data
    public void SetUsername(string username)
    {
        mUsername = username;
    }
    public string GetUsername()
    {
        return mUsername;
    }

    public void SetEquippedCharacter(Character character)
    {
        mEquipped = character;
    }

    public Character GetEquippedCharacter()
    {
        return mEquipped;
    }

    public void SetPassword(string password)
    {
        mPassword = password;
    }
    public string GetPassword()  // should be UNAVAILABLE, only use in testing
    {
        return mPassword;
    }

    public void InsertItem(Item item)
    {
        mItems.Add(item);
    }
    public List<Item> GetItems()
    {
        return mItems;
    }

    public void InsertCharacter(Character character)
    {
        mCharacters.Add(character);
    }
    public List<Character> GetCharacters()
    {
        return mCharacters;
    }

    public List<Mission> GetToDoMissions()
    {
        return mToDoMissions;
    }
    public List<Mission> GetDoneMissions()
    {
        return mDoneMissions;
    }

    public List<Setting> GetSettings()
    {
        return mSettings;
    }

    public int GetMoney()
    {
        return mMoney;
    }

    public void AddMoney(int mon)
    {
        mMoney += mon;
    }

    public void RemoveMoney(int mon)
    {
        mMoney -= mon;
    }

    // *** Functions
    /// <summary>
    /// Saves player data into file system using SavePlayer static function
    /// </summary>
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    /// <summary>
    /// Loads player data from file system into current state
    /// </summary>
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        // Load data into current state
        mUsername = data.mUsername;
        mPassword = data.mPassword;
        mItems = data.mItems;
        mCharacters = data.mCharacters;
        mToDoMissions = data.mToDoMissions;
        mDoneMissions = data.mDoneMissions;
        mSettings = data.mSettings;

        // When empty, initialise to prevent null exceptions
        if (mCharacters == null)
        {
            mCharacters = new List<Character>();
        }
        if (mItems == null)
        {
            mItems = new List<Item>();
        }
        if(mToDoMissions == null)
        {
            
            string databaseFile = Path.Combine(Application.persistentDataPath, "MissionTable.csv");

            string[] database = File.ReadAllLines(databaseFile);
            foreach (string line in database)
            {
                string[] values = line.Split(',');
                if (int.TryParse(values[0], out int dk))
                {
                    int id = int.Parse(values[0]);
                    string name = values[1];
                    string description = values[2];
                    int reward = int.Parse(values[3]);
                    int needed = int.Parse(values[4]);
                    mToDoMissions.Add(new Mission(id, name, description, reward, needed, 0));
                }
            }
        }
        if (mDoneMissions == null)
        {
            mDoneMissions = new List<Mission>();
        }
        if(mSettings == null)
        {
            string databaseFile = Path.Combine(Application.persistentDataPath, "SettingsTable.csv");
            string[] database = File.ReadAllLines(databaseFile);
            foreach (string line in database)
            {
                string[] values = line.Split(',');
                if (int.TryParse(values[0], out int dk))
                {
                    int id = int.Parse(values[0]);
                    string name = values[1];
                    string description = values[2];
                    mSettings.Add(new Setting(id, name, description, false));
                }
            }
        }



    }

    public static List<int> GetCharacterStats(string characterName)
    {
        // Get this character's stats from database
        //string oldDatabaseFile = Path.Combine(Application.dataPath, "Databases", "CharacterTableDraft1.csv");
        string databaseFile = Path.Combine(Application.persistentDataPath, "CharacterTableDraft1.csv");
        if (!File.Exists(databaseFile))
        {
            // Mark text as NA for not available and return early
            Debug.LogError($"Error: Character database file not found at {databaseFile}");
            return new List<int> { 0, 0, 0 };
        }
        string[] database = File.ReadAllLines(databaseFile);

        // Ignore line 1 in database, it's just headings
        // Find this character and thier details in database
        string[] characterData = { };
        for (int i = 1; i < database.Length; i++)
        {
            string[] data = database[i].Split(',');
            if (data[1] == characterName)
            {
                characterData = data;
                break;
            }
        }

        // Ensure character was found in database, otherwise log error and set stat values to NA for "Not available"
        if (characterData.Length == 0)
        {
            Debug.LogError($"Error: Character not found in database, name is {characterName}");
            return new List<int> { 0, 0, 0 }; ;
        }


        return new List<int> { int.Parse(characterData[3]), int.Parse(characterData[4]), int.Parse(characterData[5]) };
    }

}

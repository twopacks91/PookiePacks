using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

[System.Serializable]  // allows saving to a file
public class PlayerData
{
    // Class Variables
    private string mUsername;
    private string mPassword;
    private int mMoney;

    private List<Item> mItems;
    private List<Character> mCharacters;
    private List<Mission> mToDoMissions;
    private List<Mission> mDoneMissions;


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

        // Read missions database file to load all missions into To-Do list with zero progress
        string databaseFile = Path.Combine(Application.dataPath, "Databases", "MissionTable.csv");
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
    // Default constructor - loads player using information stored
    public PlayerData()
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
            Debug.Log("todo empty");
            string databaseFile = Path.Combine(Application.dataPath, "Databases", "MissionTable.csv");
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

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

[System.Serializable]  // allows saving to a file
public class PlayerData
{
    // Class Variables
    private string mUsername;
    private string mPassword;

    private List<Item> mItems;
    private List<Character> mCharacters;

    // *** Constructors - (uncommented for now UNTIL required - UPDATE it is now required - Hamza)
    // Create and Save new player instance - only use in registering, otherwise data will be overwritten
    public PlayerData(string username, string password)
    {
        mUsername = username;
        mPassword = password;
        mItems = new List<Item>(0);
        mCharacters = new List<Character>(0);
    }
    // Default constructor - loads player using information stored
    public PlayerData()
    {

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

        // When empty, initialise to prevent null exceptions
        if (mCharacters == null)
        {
            mCharacters = new List<Character>();
        }
        if (mItems == null)
        {
            mItems = new List<Item>();
        }
    }

}

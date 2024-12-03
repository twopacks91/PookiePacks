using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]  // allows saving to a file
public class PlayerData
{
    // Class Variables
    private string mUsername;
    private string mPassword;

    private List<Item> mInventory;

    //// *** Constructors - (uncommented for now UNTIL required - Hamza)
    //// Create and Save new player instance - only use in registering, otherwise data will be overwritten
    //public PlayerData(string username, string password)
    //{
    //    mUsername = username;
    //    mPassword = password;
    //    mInventory = new List<Item>;
    //    this.SavePlayer();
    //}
    //// Default constructor - loads player using information stored
    //public PlayerData()
    //{
    //    this.LoadPlayer();
    //}

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
        mInventory = data.mInventory;
    }
}

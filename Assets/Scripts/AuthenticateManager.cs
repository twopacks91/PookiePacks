using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Text.RegularExpressions;
using System;
using System.Collections;
using UnityEngine.Audio;

public class LoginManager : MonoBehaviour
{
    // Variables
    [SerializeField]  // private (other scripts can't access but can be seen in unity editor)
    private TMP_InputField usernameField;
    [SerializeField]
    private TMP_InputField passwordField;
    [SerializeField]
    private TMP_InputField confirmPasswordField;
    [SerializeField]
    private TextMeshProUGUI errorText;


    /// <summary>
    /// Called when submit button is pressed on login page, grabs the username and password which will then be validated
    /// ... scene switched when successful, or error message provided otherwise
    /// </summary>
    public void OnSubmitLogin()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        // Validate
        string loginValidation = ValidateLoginInfo(username, password);

        // Empty return = Successful login, so switch to home scene
        if (string.IsNullOrEmpty(loginValidation))
        {
            Debug.Log("Successful Login!");

            // Ensure player exists and save data
            try
            {
                PlayerData player = PlayerData.GetInstance();
                // Save Player Data
                player.SetUsername(username);
                player.SetPassword(password);
                player.SavePlayer();

            }
            catch (NullReferenceException exception)
            {
                Debug.LogError($"Error while tryna login - {exception.Message}");
                errorText.text = "ERROR: Perhaps you have not registered yet, click Register.";
                return;
            }

            // Load Home Screen
            SceneManager.LoadScene("Home");
        }
        else
        {
            Debug.LogError("ERROR: " + loginValidation);
            errorText.text = "ERROR: " + loginValidation;
        }
    }

    /// <summary>
    /// Called in OnSubmitLogin to validate username and password against criteria
    /// </summary>
    /// <returns> Empty string for correct log in, Otherwise error message explanation. </returns>
    private string ValidateLoginInfo(string username, string password)
    {
        string returnString = "";

        // In-depth checks done via helper function
        returnString = ValidateAgainstCriteria(username, password);

        // Important basic checks to acknowledge first (empty input)
        if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
        {
            returnString = "Both username and password are empty";
        }
        else if (string.IsNullOrEmpty(username))
        {
            returnString = "Username is empty";
        }
        else if (string.IsNullOrEmpty(password))
        {
            returnString = "Password is empty";
        }

        Debug.Log(returnString);
        return returnString;
    }

    /// <summary>
    /// Called when submit button is pressed on register page, grabs the username, password, confirm password which will then be validated
    /// ... scene switched when successful, or error message provided otherwise
    /// </summary>
    public void OnSubmitRegister()
    {
        string username = usernameField.text;
        string password = passwordField.text;
        string confirmPassword = confirmPasswordField.text;

        // Validate
        string registerValidate = ValidateRegisterInfo(username, password, confirmPassword);

        // Check that registration was successful (empty return = success)
        if (string.IsNullOrEmpty(registerValidate))
        {
            Debug.Log("Successful Register!");

            // Save Player Data
            PlayerData player = new PlayerData(username, password);
            player.SavePlayer();

            // using default constructor - expect issues
            //PlayerData player = new PlayerData();
            //player.SetUsername(username);
            //player.SetPassword(password);

            // Load Home Screen
            SceneManager.LoadScene("Home");
        }
        else
        {
            Debug.LogError("ERROR: " + registerValidate);
            errorText.text = "ERROR: " + registerValidate;
        }
    }

    /// <summary>
    /// Called in OnSubmitRegister to validate username, password against criteria
    /// </summary>
    /// <returns> Empty string for correct log in, Otherwise error message explanation. </returns>
    private string ValidateRegisterInfo(string username, string password, string confirmPassword)
    {
        string returnString = "";

        // In-depth checks done via helper function
        returnString = ValidateAgainstCriteria(username, password);

        // Important basic checks to see first (empty input)
        if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password) && string.IsNullOrEmpty(confirmPassword))
        {
            returnString = "All inputs are empty";
        }
        else if (string.IsNullOrEmpty(username))
        {
            returnString = "Username is empty";
        }
        else if (string.IsNullOrEmpty(password))
        {
            returnString = "Password is empty";
        }
        else if (string.IsNullOrEmpty(confirmPassword))
        {
            returnString = "Confirm Password is empty";
        }
        // Passwords must match
        else if (password != confirmPassword)
        {
            returnString = "Password and Confirm Password do not match";
        }

        Debug.Log(returnString);
        return returnString;
    }

    /// <summary>
    /// Validates a username and password against specific criteria
    /// </summary>
    /// <returns> Empty string for validation pass, Otherwise an error message explanation. </returns>
    private string ValidateAgainstCriteria(string username, string password)
    {
        string returnString = "";

        // Contains at least one special character (not digit or alphabetical)
        bool containsSpecial = false;
        foreach (char c in password)
        {
            if (!char.IsLetterOrDigit(c))
            {
                containsSpecial = true;
            }
        }

        // Check through password based on set criteria
        if ((username.Length < 3) || (username.Length > 30))  // username length between 3-30 inclusive
        {
            returnString = "Username must be between 3 and 30 characters";
        }
        else if (password.Length < 8)                         // password must be at least 8 characters
        {
            returnString = "Password must be at least 8 characters";
        }
        else if (!Regex.IsMatch(password, @"[a-z]"))         // password has at least one lowercase letter
        { 
            returnString = "Password must contain at least one lowercase letter";
        }
        else if (!Regex.IsMatch(password, @"[A-Z]"))        // password has at least one uppercase letter
        {
            returnString = "Password must containt at least one uppercase letter";
        }
        else if (!Regex.IsMatch(password, @"[0-9]"))       // password has at least one digit
        {
            returnString = "Password must contain at least one number";
        }
        else if (!containsSpecial)                        // password has at least one special character
        {
            returnString = "Password must contain at least one special character";
        }

        return returnString;
    }

    /// <summary>
    /// Switches to register scene when initiated
    /// </summary>
    public void GoToRegisterScene()
    {
        SceneManager.LoadScene("Register");  // UPDATE - scene name / number in build settings
    }

    /// <summary>
    /// Switches to Login scene when initiated
    /// </summary>
    public void GoToLoginScreen()
    {
        SceneManager.LoadScene("Login");  // UPDATE - scene name / number in build settings
    }

    public void GoToProfileScreen()
    {
        SceneManager.LoadScene("Profiler");  // UPDATE - scene name / number in build settings
    }

    /// <summary>
    /// Remove error when a user is typing since they're probably attempting to fix the error
    /// </summary>
    public void RemoveErrorText()
    {
        errorText.text = "";
    }
}

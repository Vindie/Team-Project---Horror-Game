using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour {

    public bool showDebugMessages = true;
    public Settings gameSettings;
    protected string settingsFilePath = "settings.binary";

	protected void Start () {
        //There can only be one
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            //I am not the original, destroy me.
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);

            InitializeSettings();
        }
	}

    protected void InitializeSettings()
    {
        if (showDebugMessages)
        {
            Debug.Log("Initializing settings.");
        }

        if (!LoadSettings())
        {
            gameSettings = new Settings();
            gameSettings.ResetToDefaultSettings();
        }

        AudioListener.volume = gameSettings.Volume;
    }

    public void SaveSettings()
    {
        if (showDebugMessages)
        {
            Debug.Log("Saving settings.");
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream settingsFile = File.Create(settingsFilePath);

        formatter.Serialize(settingsFile, gameSettings);

        settingsFile.Close();
    }

    //Returns true if there is a file to load.
    public bool LoadSettings()
    {
        if (showDebugMessages)
        {
            Debug.Log("Loading settings.");
        }

        if (!File.Exists(settingsFilePath)) { return false; }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream settingsFile = File.Open(settingsFilePath, FileMode.Open);

        gameSettings = (Settings)formatter.Deserialize(settingsFile);
        settingsFile.Close();
        return true;
    }

    /*public bool debugSensitivity = false;
    private void Update()
    {
        if(debugSensitivity)
        {
            FindObjectOfType<FPS_Pawn>().CheckSettings();
            debugSensitivity = false;
        }
    }*/
}

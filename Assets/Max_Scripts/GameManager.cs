using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour {

    public Settings gameSettings;

	protected void Start () {
        //There can only be one
        if (FindObjectOfType<GameManager>())
        {
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
        if(!LoadSettings())
        {
            gameSettings = new Settings();
            gameSettings.ResetToDefaultSettings();
        }
    }

    public void SaveSettings()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream settingsFile = File.Create("settings.binary");

        formatter.Serialize(settingsFile, gameSettings);

        settingsFile.Close();
    }

    //Returns true if there is a file to load.
    public bool LoadSettings()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream settingsFile = File.Open("settings.binary", FileMode.Open);
        if(settingsFile == null)
        {
            return false;
        }

        gameSettings = (Settings)formatter.Deserialize(settingsFile);
        settingsFile.Close();
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings {
    public float MouseSensitivity;
    public float Volume;
    public float Brightness;
    
    public void ResetToDefaultSettings()
    {
        MouseSensitivity = 1.0f;
        Volume = 1.0f;
        Brightness = 1.0f;
    }
}

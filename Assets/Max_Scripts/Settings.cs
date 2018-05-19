using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings {
    public float MouseSensitivity = 1.0f;
    public float Volume = 1.0f;
    public float Brightness = 1.0f;
    
    public void ResetToDefaultSettings()
    {
        MouseSensitivity = 1.0f;
        Volume = 1.0f;
        Brightness = 1.0f;
    }
}

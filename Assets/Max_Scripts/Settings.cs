using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings {
    public float MouseSensitivity = 0.32f;
    public float Volume = 1.0f;
    public float Brightness = 1.0f;
    
    public void ResetToDefaultSettings()
    {
        MouseSensitivity = 0.32f;
        Volume = 1.0f;
        Brightness = 1.0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSlider : MonoBehaviour
{
    public enum SettingType { MOUSESENSITIVITY, VOLUME, BRIGHTNESS };
    public SettingType ControlledSetting;
    public Text label;
    public string labelText = "Undefined";

    protected delegate void delegateMethod(float value);
    protected Dictionary<SettingType, delegateMethod> SettingDictionary;

    protected GameManager _gm;
    protected Slider _sl;

    // Use this for initialization
    protected virtual void Start()
    {
        InitializeSettingsDictionary();
        InitializeSlider();
    }

    public virtual void UpdateSetting(float value)
    {
        SettingDictionary[ControlledSetting].Invoke(value);
        UpdateLabel();
    }

    protected virtual void UpdateLabel()
    {
        if (!label) { return; }

        int percentage = (int)(100.0f * (_sl.value / 1.0f));

        label.text = labelText + ":" + percentage + "%"; 
    }

    protected virtual void InitializeSettingsDictionary()
    {
        SettingDictionary = new Dictionary<SettingType, delegateMethod>
        {
            { SettingType.MOUSESENSITIVITY, UpdateMouseSensitivity },
            { SettingType.VOLUME, UpdateVolume },
            { SettingType.BRIGHTNESS, UpdateBrightness }
        };
    }

    public void SliderCheckValue()
    {
        if(!_gm) { return;  }
        if (ControlledSetting == SettingType.MOUSESENSITIVITY)
        {
            _sl.value = _gm.gameSettings.MouseSensitivity;
        }
        else if (ControlledSetting == SettingType.VOLUME)
        {
            _sl.value = _gm.gameSettings.Volume;
        }
        else if (ControlledSetting == SettingType.BRIGHTNESS)
        {
            _sl.value = _gm.gameSettings.Brightness;
        }

        UpdateSetting(_sl.value);
    }

    protected void InitializeSlider()
    {
        if (!_gm) { _gm = GameObject.FindObjectOfType<GameManager>(); }
        _sl = gameObject.GetComponent<Slider>();
        SliderCheckValue();
    }

    #region Setting Action Methods
    protected virtual void UpdateMouseSensitivity(float value)
    {
        //Debug.Log ("Updating mouse sensitivity to " + value);
        if(!_gm) { _gm = GameObject.FindObjectOfType<GameManager>(); }
        if(_gm)
        {
            _gm.gameSettings.MouseSensitivity = value;
            FPS_Pawn[] AllFPP = FindObjectsOfType<FPS_Pawn>();
            foreach(FPS_Pawn FPP in AllFPP)
            {
                FPP.CheckSettings();
            }
        }
    }
    protected virtual void UpdateVolume(float value)
    {
        if (!_gm) { _gm = GameObject.FindObjectOfType<GameManager>(); }
        if (_gm)
        {
            //Debug.Log("Updating volume to " + value);
            _gm.gameSettings.Volume = value;
            AudioListener.volume = value;
        }
    }
    protected virtual void UpdateBrightness(float value)
    {
        //Debug.Log("Updating mouse brightness to " + value);
        if (!_gm) { _gm = GameObject.FindObjectOfType<GameManager>(); }
        if (_gm)
        {
            _gm.gameSettings.Brightness = value;
            ImageEffectManager.brightness = value;
        }
    }
    #endregion
}

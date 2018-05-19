using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSlider : MonoBehaviour {

    public enum SettingType { MOUSESENSITIVITY, VOLUME, BRIGHTNESS };
    public SettingType ControlledSetting;

    protected GameManager _gm;

	// Use this for initialization
	void Start () {
		
	}

    public virtual void UpdateSetting(float value)
    {
        if(Mouse)
    }
}

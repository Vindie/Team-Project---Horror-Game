using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject firePrefab;
    public bool LightActive
    {
        get { return _lightActive; }
    }
    protected bool _lightActive;

	void Start ()
    {
        LightOn(); 
    }

    public void LightOn()
    {
        firePrefab.SetActive(true);
        _lightActive = true;
    }

    public void LightOff()
    {
        firePrefab.SetActive(false);
        _lightActive = false;
    }

}

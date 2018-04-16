using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject firePrefab;
	void Start ()
    {
        LightOn(); 
    }

    public void LightOn()
    {
        firePrefab.SetActive(true);
    }

    public void LightOff()
    {
        firePrefab.SetActive(false); 
    }

}

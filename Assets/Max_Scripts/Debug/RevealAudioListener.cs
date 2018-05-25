using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RevealAudioListener : MonoBehaviour {

    public float alVolume;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        alVolume = AudioListener.volume;
	}
}

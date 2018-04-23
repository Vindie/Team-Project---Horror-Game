using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour {

    public Transform CameraTransform;

	// Use this for initialization
	void Start () {
		if(!CameraTransform)
        {
            GetCameraTransform();
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(CameraTransform)
        {
            transform.forward = transform.position - CameraTransform.position;
        }
        else
        {
            GetCameraTransform();
        }
	}

    protected void GetCameraTransform()
    {
        Camera cam = FindObjectOfType<Camera>();
        if(!cam) { return; }
        CameraTransform = cam.transform;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Interactable {

    public bool isLocked = false;
    public float doorSwingTorque = 1.0f;
    public float closeSensitivity = 0.5f;

    protected Rigidbody rb;

    protected Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    protected override bool ProcessInteraction(Actor source, Controller instigator)
    {
        if(isLocked)
        {
            return false;
        }

        //If under sensitivity value, door is closed and should be opened.
        if(closeSensitivity < Quaternion.Dot(initialRotation, transform.rotation))
        {
            rb.AddRelativeTorque(0.0f, doorSwingTorque, 0.0f);
        }
        else //else, close the door.
        {
            rb.MoveRotation(initialRotation);
        }

        return true;
    }
}

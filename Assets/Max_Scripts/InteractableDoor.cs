using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Interactable {

    public bool isLocked = false;
    public bool toggleOpenningDirection = true;
    public float doorSwingForce = 20.0f;
    //Infinite spring force means the door can't be pushed open. Need to set the target rotation to open things this way. Crashed unity though, so setting IsKinematic is probably better.


    protected HingeJoint hj;
    protected Rigidbody rb;

    protected float currentSwingForce;

    private void Start()
    {
        hj = gameObject.GetComponentInParent<HingeJoint>();
        rb = gameObject.GetComponent<Rigidbody>();

        currentSwingForce = doorSwingForce;
    }

    protected override bool ProcessInteraction(Actor source, Controller instigator)
    {
        if(isLocked)
        {
            return false;
        }

        //If under sensitivity value, door is closed and should be opened.
        

        return true;
    }
}

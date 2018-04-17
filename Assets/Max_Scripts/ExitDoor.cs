using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : Interactable {

    public bool isLocked = true;

    public virtual void SetDoorLock(bool value)
    {
        isLocked = value;
    }

    protected override bool ProcessInteraction(Actor source, Controller instigator)
    {
        if(isLocked)
        {
            return false;
        }
        //PlayerController
        //Fade to black
        //Fade in splash text - "Escaped the mine" or something
        MenuScript ms = FindObjectOfType<MenuScript>();
        if(!ms)
        {
            LOG_ERROR("No object in scene with MenuScript attached. Can not return to main menu.");
            return true;
        }
        ms.ReturnToMainMenu();

        return true;
    }
}

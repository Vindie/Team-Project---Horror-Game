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
        
        HorrorGame hg = FindObjectOfType<HorrorGame>();
        if(!hg)
        {
            LOG_ERROR("No object in scene with HorrorGame attached. Can not ask to end game.");
            return true;
        }
        hg.EndGame(true);

        FPS_Controller FPC = (FPS_Controller)instigator;
        if(FPC)
        {
            FPC.allowPausing = false;
        }

        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : Interactable {

    public bool isLocked = true;
    public float hintPopupTime = 5.0f;
    public string popupHint = "The exit seems to be locked, perhaps there is a key somewhere?";
    public AudioSource DoorOpen;
    MenuScript _ms;

    private void Start()
    {
        _ms = FindObjectOfType<MenuScript>();
    }

    public virtual void SetDoorLock(bool value)
    {
        isLocked = value;
    }

    protected override bool ProcessInteraction(Actor source, Controller instigator)
    {
        if(isLocked)
        {
            FPS_Pawn FPP = (FPS_Pawn)source;
            if (FPP)
            {
                if (FPP.hasKey)
                {
                    isLocked = false;
                    DoorOpen.Play();
                    return false;
                }
            }

            if (_ms)
            {
                _ms.SetGameSmallText(true, popupHint, hintPopupTime);
            }
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
            FPC.UnPossesPawn(FPC.GetPossesedPawn());
            FPC.allowPausing = false;
        }

        return true;
    }
}

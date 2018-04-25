using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : Interactable {

    public bool isLocked = true;
    public float hintPopupTime = 5.0f;
    public string popupHint = "The exit seems to be locked, perhaps there is a key somewhere?";

    protected float hintPopupTimeRemaining = 0.0f;

    public virtual void SetDoorLock(bool value)
    {
        isLocked = value;
    }

    protected override bool ProcessInteraction(Actor source, Controller instigator)
    {
        if(isLocked)
        {
            if(hintPopupTimeRemaining <= 0.0f)
            {
                StartCoroutine(ShowPopupHint());
            }
            else
            {
                hintPopupTimeRemaining = hintPopupTime;
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

    protected virtual IEnumerator ShowPopupHint()
    {
        MenuScript ms = FindObjectOfType<MenuScript>();
        if(ms)
        {
            ms.SetGameSmallText(true, popupHint);
            hintPopupTimeRemaining = hintPopupTime;
            while (hintPopupTimeRemaining > 0)
            {
                hintPopupTimeRemaining -= Time.deltaTime;
                yield return null;
            }
            ms.SetGameSmallText(false);
        }
    }
}

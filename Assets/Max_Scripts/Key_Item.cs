using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Item : Interactable {

    public float hintPopupTime = 5.0f;
    public string popupHint = "Picked up mine key";

    protected float hintPopupTimeRemaining = 0.0f;

    protected override bool ProcessInteraction(Actor source, Controller instigator)
    {
        FPS_Pawn FPP = (FPS_Pawn)source;
        if(!FPP) { return false; }

        FindObjectOfType<MenuScript>().SetGameSmallText(true, popupHint, hintPopupTime);

        FPP.hasKey = true;
        Destroy(gameObject);
        return true;
    }

    protected virtual IEnumerator ShowPopupHint()
    {
        MenuScript ms = FindObjectOfType<MenuScript>();
        if (ms)
        {
            ms.SetGameSmallText(true, popupHint);
            hintPopupTimeRemaining = hintPopupTime;
            while (hintPopupTimeRemaining > 0)
            {
                Debug.Log("Timer: " + hintPopupTimeRemaining);
                hintPopupTimeRemaining -= Time.deltaTime;
                yield return null;
            }
            LOG("Exit Popup");
            ms.SetGameSmallText(false);
        }
    }
}

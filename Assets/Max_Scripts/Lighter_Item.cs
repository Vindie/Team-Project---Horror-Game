using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter_Item : Item {

    public bool isOpen = false;

    public override bool Use(Actor user)
    {
        isOpen = !isOpen;
        //animation toggling open lighter;
        return isOpen;
    }

    public virtual bool ignite(Actor user)
    {
        if(!isOpen) { return false; }
        FPS_Pawn FPP = (FPS_Pawn)user;
        if(!FPP) { return false; }
        GameObject target = FPP.GetInteractableObject();
        if(!target) { return false; }
        Torch t = target.GetComponent<Torch>();
        t.LightOn();
    }
}

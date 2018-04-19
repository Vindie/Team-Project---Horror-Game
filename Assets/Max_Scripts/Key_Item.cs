using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Item : Item {

    public override bool Use(Actor user)
    {
        FPS_Pawn FPP = (FPS_Pawn)user;
        if(!FPP) { return false; }

        GameObject objectKeyIsBeingUsedOn = FPP.GetInteractableObject();
        if(!objectKeyIsBeingUsedOn) { return false; }

        ExitDoor door = objectKeyIsBeingUsedOn.GetComponent<ExitDoor>();
        if(!door) { return false; }

        door.isLocked = false;
        //Delete this item, or remove this component from the object.
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable {

    /// <summary>
    /// The basic interaction of an item is to have the FPS_Pawn equip it.
    /// This is the main function to override in inheriting classes.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="instigator"></param>
    /// <returns>Return true if the item is equipped</returns>
    protected override bool ProcessInteraction(Actor source, Controller instigator)
    {
        FPS_Pawn fpp = source.GetComponent<FPS_Pawn>();
        if(fpp)
        {
            return fpp.Equip(this);
        }
        return false;
    }
}

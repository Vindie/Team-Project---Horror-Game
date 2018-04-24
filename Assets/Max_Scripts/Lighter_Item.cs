using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter_Item : Item {

    public bool isOpen = false;
    Animator anim;
    public GameObject flame;

    protected override void Start()
    {
        base.Start();
        anim = gameObject.GetComponent<Animator>();
        if(anim)
        {
            anim.SetBool("IsOpen", isOpen);
        }

        if(!flame)
        {
            Light l = GetComponentInChildren<Light>();
            if(l)
            {
                flame = l.gameObject;
            }
        }
        if(flame)
        {
            flame.SetActive(isOpen);
        }
    }

    public override bool Use(Actor user)
    {
        isOpen = !isOpen;

        anim.SetBool("IsOpen", isOpen);
        flame.SetActive(isOpen);

        return isOpen;
    }

    public virtual bool Ignite(Actor user)
    {
        if(!isOpen) { return false; }

        //Light torch animation

        FPS_Pawn FPP = (FPS_Pawn)user;
        if(!FPP) { return false; }

        Torch t = (Torch)FPP.handDominant.EquippedItem;
        if(!t)
        {
            GameObject target = FPP.GetInteractableObject();
            if (!target) { return false; }

            t = target.GetComponent<Torch>();
            if (!t) { return false; }
        }
        
        if(t.LightActive) { return false; }

        t.LightOn();
        t.getLit = true;
        return true;
    }
}

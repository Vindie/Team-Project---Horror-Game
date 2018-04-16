using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpectator : Pawn
{
    MenuScript ms;

    protected virtual void Start()
    {
        ms = FindObjectOfType<MenuScript>();
        if (!ms)
        {
            LOG_ERROR("Menu Spectator could not find object of type MenuScript.");
        }
    }

    public virtual void CloseMenuSplash()
    {
        if(ms)
        {
            ms.CloseSplash();
        }
    }
}

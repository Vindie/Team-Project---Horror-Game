using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : PlayerController
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        LogInputStateInfo = false;
        LogHUDUpdateError = false;
    }

    public override void DefaultBinds()
    {
        AddAxis("LookHorizontal", NoFunction);
        AddAxis("LookVertical", NoFunction);
        AddAxis("MoveHorizontal", NoFunction);
        AddAxis("MoveVertical", NoFunction);
        AddButton("Fire1", Fire1);
        AddButton("Fire2", Fire2);
        AddButton("Fire3", Fire3);
        AddButton("Fire4", Fire4);
        AddButton("Fire5", Fire5);
        AddButton("Cancel", Cancel);
    }

    public virtual void NoFunction(float value) { }

    public override void Fire1(bool value)
    {
        MenuSpectator MS = (MenuSpectator)PossesedPawn;
        if (MS && value)
        {
            MS.CloseMenuSplash();
        }
    }

    public override void Fire2(bool value)
    {
        MenuSpectator MS = (MenuSpectator)PossesedPawn;
        if (MS && value)
        {
            MS.CloseMenuSplash();
        }
    }

    public override void Fire3(bool value)
    {
        MenuSpectator MS = (MenuSpectator)PossesedPawn;
        if (MS && value)
        {
            MS.CloseMenuSplash();
        }
    }

    public override void Fire4(bool value)
    {
        MenuSpectator MS = (MenuSpectator)PossesedPawn;
        if (MS && value)
        {
            MS.CloseMenuSplash();
        }
    }

    public virtual void Fire5(bool value)
    {
        MenuSpectator MS = (MenuSpectator)PossesedPawn;
        if (MS && value)
        {
            MS.CloseMenuSplash();
        }
    }

    public virtual void Cancel(bool value)
    {
        MenuSpectator MS = (MenuSpectator)PossesedPawn;
        if (MS && value)
        {
            MS.CloseMenuSplash();
        }
    }
}

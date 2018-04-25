using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Controller : PlayerController {

    MenuScript ms;
    public bool allowPausing = true;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        LogInputStateInfo = false;
        LogHUDUpdateError = false;

        ms = FindObjectOfType<MenuScript>();
        if (!ms)
        {
            LOG_ERROR("FPS Controller could not find object of type MenuScript.");
        }
        else
        {
            ms.FPSPawn = (FPS_Pawn)PossesedPawn;
        }
    }

    protected virtual void Update()
    {
        //Escape input needs to be done here, so that it can be detected even when timescale = 0.
        Cancel(Input.GetButtonDown("Cancel"));
    }

    #region Controls Related
    public override void DefaultBinds()
    {
        AddAxis("LookHorizontal", LookHorizontal);
        AddAxis("LookVertical", LookVertical);
        AddAxis("MoveHorizontal", Horizontal);
        AddAxis("MoveVertical", Vertical);
        AddButton("Fire1", Fire1);
        AddButton("Fire2", Fire2);
        AddButton("Fire3", Fire3);
        AddButton("Fire4", Fire4);
        AddButton("Fire5", Fire5);
    }

    public virtual void LookHorizontal(float value)
    {
        FPS_Pawn FPP = (FPS_Pawn)PossesedPawn;
        if(FPP)
        {
            FPP.LookHorizontal(value);
        }
    }

    public virtual void LookVertical(float value)
    {
        FPS_Pawn FPP = (FPS_Pawn)PossesedPawn;
        if (FPP)
        {
            FPP.LookVertical(value);
        }
    }

    public override void Horizontal(float value)
    {
        FPS_Pawn FPP = (FPS_Pawn)PossesedPawn;
        if (FPP)
        {
            FPP.MoveHorizontal(value);
        }
    }

    public override void Vertical(float value)
    {
        FPS_Pawn FPP = (FPS_Pawn)PossesedPawn;
        if (FPP)
        {
            FPP.MoveVertical(value);
        }
    }

    public override void Fire1(bool value)
    {
        FPS_Pawn FPP = (FPS_Pawn)PossesedPawn;
        if (FPP)
        {
            FPP.Fire1(value);
            if (!ms)
            {
                FPP.SetCursorLock(true);
            }
            else
            {
                if (!ms.IsPaused)
                {
                    FPP.SetCursorLock(true);
                }
            }
        }
    }

    public override void Fire2(bool value)
    {
        FPS_Pawn FPP = (FPS_Pawn)PossesedPawn;
        if (FPP)
        {
            FPP.Fire2(value);
        }
    }

    public override void Fire3(bool value)
    {
        FPS_Pawn FPP = (FPS_Pawn)PossesedPawn;
        if (FPP)
        {
            FPP.Fire3(value);
        }
    }

    public override void Fire4(bool value)
    {
        FPS_Pawn FPP = (FPS_Pawn)PossesedPawn;
        if (FPP)
        {
            FPP.Fire4(value);
        }
    }

    public virtual void Fire5(bool value)
    {
        FPS_Pawn FPP = (FPS_Pawn)PossesedPawn;
        if (FPP)
        {
            FPP.Fire5(value);
        }
    }

    public virtual void Cancel(bool value)
    {
        if(value)
        {
            FPS_Pawn FPP = (FPS_Pawn)PossesedPawn;
            if (FPP && ms && allowPausing)
            {
                ms.TogglePause();
                FPP.SetCursorLock(!ms.IsPaused);
                //LOG("Escape");
            }
        }
    }
    #endregion

    public virtual void PawnHasDied()
    {
        UnPossesPawn(PossesedPawn);
        HorrorGame hg = FindObjectOfType<HorrorGame>();
        if(!hg) { return; }

        hg.EndGame(false);
    }

    public virtual void PawnHasEscaped()
    {
        HorrorGame hg = FindObjectOfType<HorrorGame>();
        if (!hg) { return; }

        hg.EndGame(true);
    }
}

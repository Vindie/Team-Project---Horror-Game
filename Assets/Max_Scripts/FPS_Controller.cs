using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Controller : PlayerController {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        LogInputStateInfo = false;
        LogHUDUpdateError = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Unused
	}

    public override void DefaultBinds()
    {
        AddAxis("LookHorizontal", LookHorizontal);
        AddAxis("LookVertical", LookVertical);
        AddAxis("MoveHorizontal", Horizontal);
        AddAxis("MoveVertical", Vertical);
        AddButton("Fire1", Fire1);
        AddButton("Fire2", Fire2);
        AddButton("Fire3", Fire3);
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
            FPP.Interact(value);
        }
    }

    public override void Fire2(bool value)
    {
        FPS_Pawn FPP = (FPS_Pawn)PossesedPawn;
        if (FPP)
        {
            FPP.Examine(value);
        }
    }

    public override void Fire3(bool value)
    {
        FPS_Pawn FPP = (FPS_Pawn)PossesedPawn;
        if (FPP)
        {
            FPP.Crouch(value);
        }
    }
}

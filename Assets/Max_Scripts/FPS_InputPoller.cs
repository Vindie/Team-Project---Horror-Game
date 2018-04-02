using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_InputPoller : InputPoller {

    public override InputState GetPlayer1Input()
    {
        // Example Input binding. 
        InputState IS = InputState.GetBlankState();
        IS.AddAxis("LookHorizontal", Input.GetAxis("Mouse Y"));  //Mouse?
        IS.AddAxis("LookVertical", Input.GetAxis("Mouse X"));      //Mouse?
        IS.AddAxis("MoveHorizontal", Input.GetAxis("Horizontal"));
        IS.AddAxis("MoveVertical", Input.GetAxis("Vertical"));
        IS.AddButton("Fire1", Input.GetButton("Fire1"));
        IS.AddButton("Fire2", Input.GetButton("Fire2"));
        IS.AddButton("Fire3", Input.GetButton("Fire3"));
        return IS;
    }
}

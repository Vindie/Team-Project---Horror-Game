using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTAIController : AIController {

    public GameObject playerPawn;
    Vector3 movementDirection;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	//override void Update ()
    //{
     //   followPlayer();
	//}
    /*
     * create behavior functions
     * Randomly selects different behaviors
     * puts out torches in passing
     * distance check player to monster
     * tracks last known postion of player
     * do raycast cone of vision
     * hunt down player
     * if player has torch priority to snuff out torch
     * dim light with lighter
     * check player's vision to detemine if monster can snuff out light(dot product)
     * 
     * */
     public void followPlayer()
    {
        
    }
}

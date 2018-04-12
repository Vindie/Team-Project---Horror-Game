using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTAIController : AIController {

    public GameObject playerPawn;
    public int fieldOfViewDegrees = 145;
    Vector3 movementDirection;
    Vector3 locationLastPlayerSeen;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    //override void Update ()
    //{
    //   
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

    public bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 rayDirection = playerPawn.transform.position - transform.position;

        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewDegrees * 1f)
        {
            // Detect if player is within the field of view
            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    locationLastPlayerSeen = hit.point;
                    return true;
                }
                return false;
            }
        }

        return false;
    }
}

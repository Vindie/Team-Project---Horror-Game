
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTAIController : MonoBehaviour
{

    public GameObject playerPawn;
    public int fieldOfViewDegrees = 145;
    public float moveSpeed = 20.0f;
    Vector3 locationLastPlayerSeen;

    // Use this for initialization
    void Start()
    {
        locationLastPlayerSeen = playerPawn.transform.position;
    }

    public void Update()
    {
        CanSeePlayer("Player");
        moveTowards(locationLastPlayerSeen, moveSpeed);
    }
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

    public bool CanSeePlayer(string tag)
    {
        RaycastHit hit;
        Vector3 rayDirection = playerPawn.transform.position - transform.position;

        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewDegrees * 1f)
        {
            // Checks if n object with a given tag is within the given field of view
            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {
                if (hit.transform.CompareTag(tag))
                {
                    locationLastPlayerSeen = hit.point;
                    return true;
                }
                return false;
            }
        }

        return false;
    }
    public void moveTowards(Vector3 LocationToMoveTowards, float moveSpeed)
    {
        transform.position = Vector3.MoveTowards(transform.position, LocationToMoveTowards, moveSpeed * Time.deltaTime);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTAIController : MonoBehaviour
{

    public GameObject playerPawn;
    public int fieldOfViewDegrees = 180;
    public int damageFactor = 10;
    public float moveSpeed = 20.0f;
    public float armsReach = 10.0f;
    Vector3 locationLastPlayerSeen;
    Vector3 torchLocation;


    // Use this for initialization
    void Start()
    {
        locationLastPlayerSeen = playerPawn.transform.position;
    }

    public void Update()
    {
        CanSeePlayer("Player");
    }

    public void FixedUpdate()
    {
        if (CanSeeTorch("Torch"))
        {
            moveTowards(torchLocation, moveSpeed);
        }
        moveTowards(locationLastPlayerSeen, moveSpeed);
        checkDamageDistance();
    }
    /*
     * puts out torches in passing
     * if player has torch priority to snuff out torch
     * dim light with lighter
     * check player's vision to detemine if monster can snuff out light(dot product)
     * */

    public bool CanSeePlayer(string tag)
    {
        RaycastHit hit;
        Vector3 rayDirection = playerPawn.transform.position - transform.position;

        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewDegrees * 1f)
        {
            // Checks if an object with a given tag is within the given field of view
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

    public bool CanSeeTorch(string tag)
    {
        RaycastHit hit;
        Vector3 rayDirection = playerPawn.transform.position - transform.position;

        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewDegrees * 1f)
        {
            // Checks if an object with a given tag is within the given field of view
            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {
                if (hit.transform.CompareTag(tag))
                {
                    torchLocation = hit.point;
                    print("ye");
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

    public void checkDamageDistance()
    {
        Pawn pp = playerPawn.GetComponent<Pawn>();
        float distanceToPlayer = Vector3.Distance(playerPawn.transform.position, gameObject.transform.position);
        print("Distance to player: " + distanceToPlayer);

        if (distanceToPlayer < armsReach)
        {
            print("Distance to player: " + distanceToPlayer);
            pp.TakeDamage(gameObject.GetComponent<Actor>(), damageFactor);
        }
    }
}

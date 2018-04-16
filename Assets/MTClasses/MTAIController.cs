
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTAIController : MonoBehaviour
{

    public GameObject playerPawn;
    public int fieldOfViewDegrees = 180;
    public int damageFactor = 10;
    public int locationIndex = 0;
    public float moveSpeed = 20.0f;
    public float armsReach = 10.0f;
    public int sightRadius = 10;
    Vector3 locationLastPlayerSeen;
    Vector3[] locations;
    Vector3 torchLocation;
    Collider[] torches;


    // Use this for initialization
    void Start()
    {
        locationLastPlayerSeen = playerPawn.transform.position;
    }

    public void Update()
    {
        CanSeePlayer("Player"); //if cant see player move randomly
    }

    public void FixedUpdate()
    {
        putTorchesOut();

        if(!CanSeePlayer("Player"))
        {
            moveTowards(locations[locationIndex], moveSpeed);
        }

        moveTowards(locationLastPlayerSeen, moveSpeed);

        checkDamageDistance();
    }

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

    public void moveTowards(Vector3 LocationToMoveTowards, float moveSpeed)
    {
        transform.position = Vector3.MoveTowards(transform.position, LocationToMoveTowards, moveSpeed * Time.deltaTime);
    }

    public void checkDamageDistance()
    {
        Pawn pp = playerPawn.GetComponent<Pawn>();
        float distanceToPlayer = Vector3.Distance(playerPawn.transform.position, gameObject.transform.position);
        print("Distance to player: " + distanceToPlayer);
        if (pp)
        {
            if (distanceToPlayer < armsReach)
            {
                //print("Distance to player: " + distanceToPlayer);
                pp.TakeDamage(gameObject.GetComponent<Actor>(), damageFactor);
            }
        }
    }

    public void putTorchesOut()
    {
        int index = 0;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightRadius);
        while (index < hitColliders.Length)
        {
            if (hitColliders[index].gameObject.tag == "Torch")
            {
                Torch ts = hitColliders[index].gameObject.GetComponent<Torch>();
                if (ts)
                {
                    print("Got Light");
                    ts.LightOff();                    
                }        
            }

            index++;
        }
    }
}

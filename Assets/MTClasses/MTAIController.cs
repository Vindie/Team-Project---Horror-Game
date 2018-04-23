
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MTAIController : AIController
{
    public GameObject playerPawn;
    public int fieldOfViewDegrees = 180;
    public int damageFactor = 10;
    public int sightRadius = 10;
    public float moveSpeed = 20.0f;
    public float armsReach = 10.0f;
    Vector3 locationLastPlayerSeen;
    public int locationIndex = 0;
    public Transform[] locations;
    Vector3 torchLocation;
    //NavMeshAgent agent;

    
    // Use this for initialization
    protected override void Start()
    {
        locationLastPlayerSeen = playerPawn.transform.position;
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    public override void Update()
    {
        CanSeePlayer("Player"); //if cant see player move randomly
    }

    public void FixedUpdate()
    {
        putTorchesOut();
        if (!CanSeePlayer("Player"))
        {
            moveToRandomLocations();
        }
        else
        { 
            moveTowards(locationLastPlayerSeen, moveSpeed);
        }
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
        if(agent)
        {
            Debug.Log("LocationToMoveTowards: " + LocationToMoveTowards);
            //Vector3 movepoint = new Vector3(-691.58f, 124f, -110f);
            //agent.SetDestination(movepoint);
            //PlaceMoveToSphereAt(movepoint); 
            agent.SetDestination(LocationToMoveTowards);
            //PlaceMoveToSphereAt(LocationToMoveTowards);
            agent.speed = moveSpeed * 5;
            //Debug.Log(agent.destination);
            //Debug.Log(agent.speed);
        }
        //agent.isStopped = true; 

        //transform.position = Vector3.MoveTowards(transform.position, LocationToMoveTowards, moveSpeed * Time.deltaTime);
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
                print("Get Hurt");
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

    public void moveToRandomLocations()
    {
        if (agent.remainingDistance <= 2)
        {
            moveTowards(locations[locationIndex].position, moveSpeed);
        Debug.Log("(" + locationIndex + ") :" + agent.destination);
        Debug.Log("remainingDistance: " + agent.remainingDistance);
       
            locationIndex++;
            if (locationIndex >= locations.Length)
            {
                locationIndex = 0; 
            }
        }
    }
}

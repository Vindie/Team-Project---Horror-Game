
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MTAIController : AIController
{
    public GameObject eye;
    public GameObject playerPawn;
    Animator Eanimator;
    public int fieldOfViewDegrees = 180;
    public int damageFactor = 10;
    public int sightRadius = 10;
    public float moveSpeed = 20.0f;
    public float armsReach = 10.0f;
    Vector3 locationLastPlayerSeen;
    public int locationIndex = 0;
    public Transform[] locations;
    Vector3 torchLocation;
    Vector3 playerPosition;
    public Vector3 oppositeQuad;
    public bool movingTowardsPlayer;
    public bool movingTowardsOppQuad;
    public Transform[] Quadrants;
    //NavMeshAgent agent;


    // Use this for initialization
    protected override void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        Eanimator = eye.GetComponent<Animator>();
        Eanimator.SetBool("IsOpen", false);
        movingTowardsPlayer = true;
        movingTowardsOppQuad = false;
        getOppositeQuadrant();
    }

    public override void Update()
    {
        if(playerPawn == null)
        {
            playerPawn = GameObject.FindGameObjectWithTag("Player");
            locationLastPlayerSeen = playerPawn.transform.position;
            playerPosition = playerPawn.transform.position;
        }

        CanSeePlayer("Player"); //if cant see player move randomly
    }

    public void FixedUpdate()
    {
        putTorchesOut();
        /*
         *         if (!CanSeePlayer("Player"))
        {
            moveToRandomLocations();
            Eanimator.SetBool("IsOpen", false);
            //print("IsOpen == false");
        }
        else
        { 
            moveTowards(locationLastPlayerSeen, moveSpeed);
            Eanimator.SetBool("IsOpen",true);
            //print("IsOpen == true");
        }
         * */

        if (movingTowardsPlayer)
        {
            Eanimator.SetBool("IsOpen", true);
            moveTowards(playerPosition, moveSpeed);
        }

        if(movingTowardsOppQuad)
        {
            Eanimator.SetBool("IsOpen", false);
            getOppositeQuadrant();
            moveTowards(oppositeQuad, moveSpeed);
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
            if (Physics.Raycast(transform.position, rayDirection, out hit, 100.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore))  //Added QueryTriggerInteraction.Ignore because raycast by default can hit triggers, which was making the monster not see the player very well.
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
        FPS_Pawn pp = playerPawn.GetComponent<FPS_Pawn>();
        float distanceToPlayer = Vector3.Distance(playerPawn.transform.position, gameObject.transform.position);
        //print("Distance to player: " + distanceToPlayer);
        if (pp)
        {
            //print("got pp");
            if (distanceToPlayer < armsReach)
            {
                print("Distance to player: " + distanceToPlayer);
                pp.TakeDamage(gameObject.GetComponent<Actor>(), damageFactor);
                //print("Get Hurt");
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
                    //print("Got Light");
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
        //Debug.Log("(" + locationIndex + ") :" + agent.destination);
        //Debug.Log("remainingDistance: " + agent.remainingDistance);
       
            locationIndex++;
            if (locationIndex >= locations.Length)
            {
                locationIndex = 0; 
            }
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            movingTowardsPlayer = false;
            movingTowardsOppQuad = true;
        }

        if(col.gameObject.tag == "OppQuad")
        {
            movingTowardsPlayer = true;
            movingTowardsOppQuad = false;
        }
    }

    public void getOppositeQuadrant()
    {
        float farthestQuadDistance = getDistanceTo(Quadrants[0].position);
        float currentQuadDistance = getDistanceTo(Quadrants[0].position);
        int currentQuad = 0;
        for (int index = 0;index<Quadrants.Length;index++)
        {
            currentQuadDistance = getDistanceTo(Quadrants[index].position);
            if (farthestQuadDistance < currentQuadDistance)
            {
                farthestQuadDistance = currentQuadDistance;
                currentQuad = index;
            }
        }

        oppositeQuad = Quadrants[currentQuad].position;       
    }

    public float getDistanceTo(Vector3 location)
    {
        float distance = -1;
        distance = Vector3.Distance(location, gameObject.transform.position);
        return distance;
    }
}


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
    public GameObject oppositeQuad;
    public GameObject playerQuad;
    public bool movingTowardsPlayer = true;
    public bool movingTowardsOppQuad = false;
    public GameObject[] Quadrants;
    public float maxTime = 60.0f;
    public float timer;
    public bool playerInArms;

    // Use this for initialization
    protected override void Start()
    {
        //LOG(playerPawn.name);
        playerPawn = GameObject.FindGameObjectWithTag("Player");
        locationLastPlayerSeen = playerPawn.transform.position;
        agent = gameObject.GetComponent<NavMeshAgent>();
        Eanimator = eye.GetComponent<Animator>();
        Eanimator.SetBool("IsOpen", false);
        movingTowardsPlayer = true;
        movingTowardsOppQuad = false;
        getOppositeQuadrant();
        timer = maxTime;
        playerInArms = false;
        getPlayerQuad();
        getOppositeQuadrant();
        //tortureTimer = maxTortureTime;
        //LOG(playerPawn.name);
    }

    public override void Update()
    {
        //UnityEngine.Debug.LogError("error");
        if (playerPawn == null)
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
        playerPosition = playerPawn.transform.position;
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
            for (int i = 0; i < agent.path.corners.Length - 1; i++)
            {
                Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red, 5);
            }
            //Debug.Log("Movingtoplayer");
            //getPlayerQuad();
            //Debug.Log("Player Quad " + playerQuad);
            Eanimator.SetBool("IsOpen", true);
            //Debug.Log("MOVING TO player");
            
            if (playerInArms == false)
            {
                if (CanSeePlayer("Player"))//if can see player
                {
                    Debug.Log("CAN see player");
                    moveTowards(locationLastPlayerSeen, moveSpeed);
                    Eanimator.SetBool("IsOpen", true);
                }
                else//if can see player
                {
                    Debug.Log("Cant see player");
                    //agent.SetDestination(playerQuad.transform.position);
                    moveTowards(playerPawn.transform.position, moveSpeed); //move towards player quad
                    //Debug.Log("Player move tooooo"+ playerQuad);
                 
                    if ((getDistanceTo(playerQuad.transform.position) < armsReach))
                    {
                        Debug.Log("Touched Player Quad updating new player quad");
                        //getPlayerQuad();
                    }
                }

                //Debug.Log("MOVING TOWARDS PLAYER");
                //moveTowards(playerPosition, moveSpeed);
                if (getDistanceTo(playerPawn.transform.position) < armsReach)
                {
                    Debug.Log("TOUCHING player");
                    playerInArms = true;
                }
            }

            if (playerInArms == true)
            {
                if (getDistanceTo(playerPawn.transform.position) >= armsReach)
                {
                    Debug.Log("No LONGer touching player");
                    playerInArms = false;
                    movingTowardsPlayer = false;
                    movingTowardsOppQuad = true;
                    getOppositeQuadrant();
                    timer = maxTime;
                }
            }
        }

        if (movingTowardsOppQuad && timer >= 0)
        {
            Eanimator.SetBool("IsOpen", false);
            timer -= Time.deltaTime;
            Debug.Log("MOVING TOWARDS OPPQUAD");
            moveTowards(oppositeQuad.transform.position, moveSpeed);
            if (getDistanceTo(oppositeQuad.transform.position) < armsReach)
            {
                movingTowardsPlayer = true;
                movingTowardsOppQuad = false;
                timer = maxTime;
                Debug.Log("TOUCHING QUAD");
            }
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
        if (agent)
        {
            Debug.Log(agent.pathStatus);
            if (!agent.pathPending && LocationToMoveTowards != agent.destination)
            {
                Debug.Log(agent.pathStatus);
                agent.SetDestination(LocationToMoveTowards);
            }          
            agent.speed = moveSpeed;
            Debug.Log(LocationToMoveTowards);
        }
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


    public void getOppositeQuadrant()
    {
        float farthestQuadDistance = getDistanceTo(Quadrants[0].transform.position);
        float currentQuadDistance = getDistanceTo(Quadrants[0].transform.position);
        int currentQuad = 0;
        for (int index = 0; index < Quadrants.Length; index++)
        {
            currentQuadDistance = getDistanceTo(Quadrants[index].transform.position);
            if (farthestQuadDistance < currentQuadDistance)
            {
                farthestQuadDistance = currentQuadDistance;
                currentQuad = index;
            }
        }

        oppositeQuad = Quadrants[currentQuad];
        //oppositeQuad.tag = "OppQuad";
    }

    public void getPlayerQuad()
    {
        float closestQuadDistance = getDistanceTo(locations[0].position);
        float currentQuadDistance = getDistanceTo(locations[0].position);
        int currentQuad = 0;
        for (int index = 0; index < locations.Length; index++)
        {
            currentQuadDistance = getDistanceTo(locations[index].position);
            if (closestQuadDistance < currentQuadDistance)
            {
                closestQuadDistance = currentQuadDistance;
                currentQuad = index;
            }
        }
        playerQuad = locations[currentQuad].gameObject;
    }

    public float getDistanceTo(Vector3 location)
    {
        float distance = -1;
        distance = Vector3.Distance(location, gameObject.transform.position);
        return distance;
    }
}

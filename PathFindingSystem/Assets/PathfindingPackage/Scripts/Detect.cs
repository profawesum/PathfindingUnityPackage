using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine;


//used to detect audio and visual sources
public class Detect : MonoBehaviour
{

    [Header("Player Detection")]
    [SerializeField] WaypointNavigator navigator;

    //player and AI components
    public GameObject player;
    public NavMeshAgent agent;

    //raycast view length
    public float viewLength = 5;

    //whether the ai should look at the player or not
    public bool lookAt;

    //used for navmesh raycast
    public Transform target;
    private bool blocked = false;

    //used to see if the ai is an advanced one
    public bool isadvancedAI;

    //also used in player detection
    public float distance;
    public float maxDistance = 10;


    //what to do when the scene loads
    private void Start()
    {
      
        //get the movement component
        navigator = this.GetComponent<WaypointNavigator>();
        //find the navmesh agent component
        agent = this.GetComponent<NavMeshAgent>();
        //find the player
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
    }


    private void Update()
    {

        //get the players position - this position
        Vector3 targetDir = player.transform.position - this.transform.position;
        //get angle between playe and this transform
        float angle = Vector3.Angle(targetDir, this.transform.forward);

        //if the player has been seen then follow the player
        if (!navigator.playerNotSeen)
        {
            agent.destination = player.transform.position;
        }

        //navmesh detection
        NavMeshHit navHit;
        //checks to see if it is an advanced AI
        if (isadvancedAI)
        {
            //get the distance between the player and the AI
            distance = Vector3.Distance(player.transform.position, this.transform.position);

            //check to see if the ray is being blocked by something
            blocked = NavMesh.Raycast(transform.position, target.position, out navHit, NavMesh.AllAreas);
            //draw a debug line
            Debug.DrawLine(transform.position, target.position, blocked ? Color.red : Color.green);
            //if the ray is blocked or the distance is greater than the max distance
            if (blocked || distance >= maxDistance)
            {
                //draw some debug lines
                Debug.DrawRay(navHit.position, Vector3.up, Color.red);
                Debug.DrawLine(transform.position, target.position,Color.red);
                //make it so the ai cannot see the player
                navigator.playerNotSeen = true;
            }
            //if the ray is not blocked and the distance is less than the max distance then the ai can see the player
            if (!blocked && distance <= maxDistance) {
                SeenPlayer();
            }
        }
    }

    //what to do when the player has been seen by the enemy
    void SeenPlayer()
    {
        //makes it so it has seen the player
        navigator.playerNotSeen = false;

        //look at the player
        transform.LookAt(player.transform.position);

        //move towards the player
        agent.destination = player.transform.position;
    }
}
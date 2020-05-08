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


    //what to do when the scene loads
    private void Start()
    {
        //get the movement component
        navigator = this.GetComponent<WaypointNavigator>();
        //find the navmesh agent component
        agent = this.GetComponent<NavMeshAgent>();
        //find the player
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update() {

        //get the players position - this position
        Vector3 targetDir = player.transform.position - this.transform.position;
        //get angle between playe and this transform
        float angle = Vector3.Angle(targetDir, this.transform.forward);

        //if the player has been seen then follow the player
        if (!navigator.playerNotSeen)
        {
            agent.destination = player.transform.position;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, 25.0f);
        int i = 0;
        while (i < hits.Length)
        {
            if (hits[i].name == "Player")
            {

                if (angle < 45.0f)
                {
                    lookAt = true;
                }
                else
                {
                    lookAt = false;
                }
                transform.LookAt(player.transform);
                //makes it so it has seen the player
                // navigator.playerNotSeen = false;

                //look at the player
                transform.LookAt(player.transform.position);

                //move towards the player
                agent.destination = player.transform.position;
                //what to do when the player has been seen
                SeenPlayer();
            }
            //increase the iterator
            i++;
        }
    }

    //on trigger enter make the ai see the player
    //lazy way of doing it
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            SeenPlayer();
        }
    }


    //when the player leaves the trigger zone make it so the ai cannot see the player
    //lazy way of doing it
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") {
            navigator.playerNotSeen = true;
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
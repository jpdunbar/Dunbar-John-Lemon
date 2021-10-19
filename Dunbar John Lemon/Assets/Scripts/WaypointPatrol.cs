using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    int m_CurrentWaypointIndex;
    public Vector3 scale;
    public TextMeshProUGUI ghostCount;
    public int numberGhosts;

    void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position);
        numberGhosts = 9;
        //ghostCount.text = "Ghosts: " + numberGhosts.ToString();
    }

    void Update()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }

    //Move the enemy if they trigger the invisible wall
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Explosion"))
        {
            //numberGhosts -= 1;
            //Destroy(gameObject, .3f);
            //ghostCount.text = "Ghosts: " + numberGhosts.ToString();
        }
    }
}

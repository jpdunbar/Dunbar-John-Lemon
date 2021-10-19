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
    public Rigidbody pickup;
    public Transform ghost;
    private Vector3 movement;

    void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position);
        movement = new Vector3(0f, 0.5f, 0f);
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
            Destroy(gameObject);
            Rigidbody pickupInstance;
            pickupInstance = Instantiate(pickup, ghost.position + movement, ghost.rotation) as Rigidbody;
        }
    }
}

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
    public Rigidbody ghostShot;
    public Transform ghost;
    public GameObject player;
    public GameObject ghostObject;
    private Vector3 movement;
    private Vector3 movement2;
    private bool check;
    private float timer;

    void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position);
        movement = new Vector3(0f, 0.5f, 0f);
        movement2 = new Vector3(0f, 2.0f, 0f);
        check = false;
        timer = 2.0f;
    }

    void Update()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
        if(check == true)
        {
            ghostObject.transform.LookAt(player.transform.position);
            if (timer >= 2.0f)
            {
                Rigidbody pickupInstance;
                pickupInstance = Instantiate(ghostShot, ghost.position + movement, ghost.rotation) as Rigidbody;
                pickupInstance.AddForce(ghost.forward * 500);
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Explosion"))
        {
            Destroy(gameObject);
            Rigidbody pickupInstance;
            pickupInstance = Instantiate(pickup, ghost.position + movement, ghost.rotation) as Rigidbody;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            check = true;
        }
    }
}

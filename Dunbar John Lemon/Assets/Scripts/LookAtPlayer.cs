using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform ghost;
    public GameObject ghostObject;
    public Rigidbody pickup;
    public GameObject marker;
    private Vector3 movement;

    void Start()
    {
        movement = new Vector3(0f, 0.5f, 0f);
        marker.SetActive(true);
    }

    void Update()
    {
        if(ghostObject != null)
        {
            transform.position = ghost.position;
        }
    }

        private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Explosion"))
        {
            if(ghostObject != null)
            {
                marker.SetActive(false);
                Rigidbody pickupInstance;
                pickupInstance = Instantiate(pickup, ghost.position + movement, ghost.rotation) as Rigidbody;
                Destroy(ghostObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrenadeAction : MonoBehaviour
{

    public Vector3 scale;
    public bool explosionTime = false;
    public float explosionSize = 0f;
    private Rigidbody rb;
    public TextMeshProUGUI ghostCount;
    public int numberGhosts;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        numberGhosts = 9;
        ghostCount.text = "Ghosts: " + numberGhosts.ToString();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (explosionSize < 5 && explosionTime == true)
        {
            explosionSize += Time.deltaTime * 18.0f;
            scale = new Vector3(explosionSize, explosionSize, explosionSize);
            transform.localScale = scale;
            rb.useGravity = false;
            rb.velocity = new Vector3(0, 0, 0);
            Destroy(gameObject, 0.4f);
        }
    }

    //Move the enemy if they trigger the invisible wall
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ExplodeGrenade"))
        {
            scale = new Vector3(5.0f, 5.0f, 5.0f);
            transform.localScale = scale;
            explosionTime = true;
        }
        else
        {
            scale = new Vector3(0.1f, 0.1f, 0.1f);
            transform.localScale = scale;
        }
        if (other.gameObject.CompareTag("Ghost"))
        {
            other.gameObject.SetActive(false);
            numberGhosts -= 1;
            ghostCount.text = "Ghosts: " + numberGhosts.ToString();
        }
    }
}

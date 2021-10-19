using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;

    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Animator m_Animator;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    public Rigidbody grenade;
    public Transform lemon;
    public Transform virtualCamera;
    public TextMeshProUGUI ghostCount;
    public GameObject win;
    public GameObject introScreen;
    private int numberGhosts;
    private bool intro = true;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Rigidbody = GetComponent<Rigidbody>();
        numberGhosts = 10;
        ghostCount.text = "Ghosts: " + numberGhosts.ToString();
        win.SetActive(false);
        introScreen.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody grenadeInstance;
            grenadeInstance = Instantiate(grenade, lemon.position, lemon.rotation) as Rigidbody;
            grenadeInstance.AddForce(lemon.forward * 500);
            intro = false;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        if(intro == false)
        {
            introScreen.SetActive(false);
        }

        if(numberGhosts <= 0)
        {
            win.SetActive(true);
        }
    }

    void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude*2);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CameraChange"))
        {
            virtualCamera.rotation = Quaternion.Euler(45, 0, 0);
        }
        if (other.gameObject.CompareTag("CameraReturn"))
        {
            virtualCamera.rotation = Quaternion.Euler(25, 0, 0);
        }
        if (other.gameObject.CompareTag("Pickup"))
        {
            numberGhosts -= 1;
            ghostCount.text = "Ghosts: " + numberGhosts.ToString();
            other.gameObject.SetActive(false);
        }
    }
}

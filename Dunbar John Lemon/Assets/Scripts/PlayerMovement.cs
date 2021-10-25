using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Image background;
    public TextMeshProUGUI livesCount;
    public GameObject win;
    public GameObject lose;
    public GameObject map;
    public GameObject playerLocation;
    public GameObject introScreen;
    private int numberGhosts;
    private int sprint;
    private int lives;
    private float reset;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Rigidbody = GetComponent<Rigidbody>();
        lives = 5;
        numberGhosts = 10;
        ghostCount.text = "Ghosts: " + numberGhosts.ToString();
        livesCount.text = "Lives: " + lives.ToString();
        win.SetActive(false);
        lose.SetActive(false);
        map.SetActive(true);
        background.enabled = true;
        playerLocation.SetActive(true);
        introScreen.SetActive(true);
        sprint = 2;
        reset = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Throw the capture device a certain distance in front of John Lemon
        if (Input.GetButtonDown("Shoot"))
        {
            Rigidbody grenadeInstance;
            grenadeInstance = Instantiate(grenade, lemon.position, lemon.rotation) as Rigidbody;
            grenadeInstance.AddForce(lemon.forward * 500);
        }

        //Enable or disable the background
        if(intro == true)
        {
            if (Input.GetButtonDown("Intro"))
            {
                background.enabled = false;
                introScreen.SetActive(false);
            }
        }
        else
        {
            if (Input.GetButtonDown("Intro"))
            {
                background.enabled = true;
                introScreen.SetActive(true);
            }
        }

        //Have the player run
        if (Input.GetButtonDown("Run"))
        {
            sprint = 4;
        }
        if (Input.GetButtonUp("Run"))
        {
            sprint = 2;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        m_Animator.SetBool("IsWalking", isWalking);

        //Animation if the player is walking
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

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        //Count and update the number of ghosts
        if(numberGhosts <= 0 && lives > 0)
        {
            win.SetActive(true);
            reset += Time.deltaTime;
        }

        //Count and update the number of lives
        if(lives <= 0)
        {
            lose.SetActive(true);
            reset += Time.deltaTime;
        }

        //Reset the game
        if(reset >= 3)
        {
            SceneManager.LoadScene(0);
        }
    }

    void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude*sprint);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Change the camera
        if (other.gameObject.CompareTag("CameraChange"))
        {
            virtualCamera.rotation = Quaternion.Euler(45, 0, 0);
        }

        //Return the camera
        if (other.gameObject.CompareTag("CameraReturn"))
        {
            virtualCamera.rotation = Quaternion.Euler(25, 0, 0);
        }

        //Pickup an item
        if (other.gameObject.CompareTag("Pickup"))
        {
            numberGhosts -= 1;
            ghostCount.text = "Ghosts: " + numberGhosts.ToString();
            other.gameObject.SetActive(false);
        }

        //Shot by an enemy
        if (other.gameObject.CompareTag("EnemyShot"))
        {
            lives -= 1;
            livesCount.text = "Lives: " + lives.ToString();
            other.gameObject.SetActive(false);
        }
    }
}

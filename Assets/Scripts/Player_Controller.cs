using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {



    [SerializeField] private Transform trans;
    [SerializeField] private Rigidbody rb;

    //max speed of character
    private float maxSpeed;
    //radius where character has reached target
    private float radiusOfSat;
    //speed that the player turns
    private float turnSpeed;
    //the point that the player should be heading to
    private Vector3 targetPoint;

    //variable to save postion of mouse click
    private Vector3 endpoint;



    private Animator anim;

    PlayerHealth health;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<PlayerHealth>();
        maxSpeed = 8f;
        radiusOfSat = 3f;
        turnSpeed = 5f;
        targetPoint = Vector3.zero;
        endpoint = trans.position;
    }

    // Update is called once per frame
    void Update()
    {
        //printVelocity();
        playerMovement();
        playerCombat();

    }

    //debugging only
    private void printVelocity()
    {
        print("current velocity: " + rb.velocity);
    }

    private void playerMovement()
    {
        
        //resets target point on each frame
        targetPoint = Vector3.zero;
        //new end point is only saved if mouse is clicked
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
            {
                endpoint = hit.point;
            }
        }
        //sets target point to equals mouse click location
        targetPoint += endpoint;
        //calculate vector to travel along using current position and target position
        Vector3 towards = targetPoint - trans.position;

        // If we haven't reached the target yet
        if (towards.magnitude > radiusOfSat)
        {
            anim.SetFloat("Speed", towards.magnitude);
            // Normalize vector to get just the direction
            towards.Normalize();
            towards *= maxSpeed;

            // Move character
            rb.velocity = towards;

            Quaternion targetRotation = Quaternion.LookRotation(towards);
            

            trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);


        } else //player rotates randomly without else statement more testing needed
        {
            anim.SetFloat("Speed", 0);
            rb.velocity = Vector3.zero;
        }
    }

    private void playerCombat()
    {
        //key commands to have player attack
        //print statements used to make sure commands are being registered while no animations
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            anim.SetBool("Punching", true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            
            anim.SetBool("Slashing", true);
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            anim.SetBool("Punching", false);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            anim.SetBool("Slashing", false);
        }

        //only use for testing
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            health.TakeDamage(50);
        }

    }

     //will be used when walls are added
    public void OnCollisionEnter(Collision col)
    {
        //stops player from moving when colliding with a wall
        if (col.gameObject.tag == "Wall")
        {
            //debugging only
            //print("wall");
            rb.velocity = Vector3.zero;
        }
    }
}

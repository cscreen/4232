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

    // Use this for initialization
    void Start()
    {
        maxSpeed = 8f;
        radiusOfSat = 3f;
        turnSpeed = 5f;
        targetPoint = Vector3.zero;
        trans.position = GameObject.FindGameObjectWithTag("PlayerStart").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //printVelocity();
        playerMovement();

    }

    //debugging only
    private void printVelocity()
    {
        print("current velocity: " + rb.velocity);
    }

    private void playerMovement()
    {
        //resets target point on each frame
        if (trans.position != GameObject.FindGameObjectWithTag("PlayerStart").transform.position)
        {
            targetPoint = Vector3.zero;
        }
        else
        {
            targetPoint = GameObject.FindGameObjectWithTag("PlayerStart").transform.position;
        }
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

        //rotates player to look at target location
        Quaternion targetRotation = Quaternion.LookRotation(towards);
        trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);


        // If we haven't reached the target yet
        if (towards.magnitude > radiusOfSat)
        {

            // Normalize vector to get just the direction
            towards.Normalize();
            towards *= maxSpeed;

            // Move character
            rb.velocity = towards;
        } else //player rotates randomly without else statement more testing needed
        {
            rb.velocity = Vector3.zero;
        }
    }

    /* will be used when walls are added
    public void OnCollisionEnter(Collision col)
    {
        //stops player from moving when colliding with a wall
        if (col.gameObject.tag == "Wall")
        {
            //debugging only
            //print("wall");
            rb.velocity = Vector3.zero;
        }
    }*/
}

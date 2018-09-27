using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    //action enum to pick which action the player will do
    enum Action
    {
        Punch = 2,
        Ax = 3,
        Gun = 5,
        Sword = 4
    };

    [SerializeField] private Transform trans;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private Transform zombieTrans;
    [SerializeField] private Rigidbody zombieRb;
 
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

    //NOTE: may move this to a game controller file
    private int health;

    //distance to zombie to be within range and cause damage
    private float withinRange;

    //current action the player is taking
    private Action currentAct;

    //amount of damage to deal to zombie when in range
    private int damageToDeal;

    private Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        health = 100;
        maxSpeed = 8f;
        radiusOfSat = 3f;
        withinRange = 4f;
        turnSpeed = 5f;
        damageToDeal = 0;
        targetPoint = Vector3.zero;
        trans.position = GameObject.FindGameObjectWithTag("PlayerStart").transform.position;
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

        // If we haven't reached the target yet
        if (towards.magnitude > radiusOfSat)
        {
            anim.SetFloat("Speed", towards.magnitude);
            // Normalize vector to get just the direction
            towards.Normalize();
            towards *= maxSpeed;

            // Move character
            rb.velocity = towards;

            //rotates player to look at target location
            Quaternion targetRotation = Quaternion.LookRotation(towards);
            trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);
        } else //player rotates randomly without else statement more testing needed
        {
            anim.SetFloat("Speed", 0);
            rb.velocity = Vector3.zero;
            //rotates player to look at target location
            Quaternion targetRotation = Quaternion.LookRotation(towards);
            trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    private void playerCombat()
    {
        //key commands to have player attack
        //print statements used to make sure commands are being registered while no animations
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentAct = Action.Punch;
            damageToDeal = 50;
            print("punch");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentAct = Action.Ax;
            damageToDeal = 3;
            print("ax");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentAct = Action.Gun;
            damageToDeal = 5;
            print("gun");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentAct = Action.Sword;
            damageToDeal = 4;
            print("sword");
        }

        //if within range of zombie cause damage to zombie
        Vector3 vecToZombie = trans.forward - zombieTrans.forward;
        if (vecToZombie.magnitude > withinRange)
        {
            //damage zombie using damageToDeal
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

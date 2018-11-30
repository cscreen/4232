using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Controller : MonoBehaviour {

    [SerializeField] private Transform trans;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private Transform playerTrans;
    [SerializeField] private Rigidbody playerRb;

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

    private float range;

    private Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        maxSpeed = 10f;
        radiusOfSat = 1.5f;
        range = 20f;
        turnSpeed = 5f;
        targetPoint = Vector3.zero;
        endpoint = trans.position;
    }
	
	// Update is called once per frame
	void Update () {
        movement();
	}

    private void movement()
    {
        var heading = playerTrans.position - trans.position;

        if(heading.sqrMagnitude < range * range)
        {
            targetPoint = Vector3.zero;
            endpoint = playerTrans.position;

            targetPoint += endpoint;

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
            }
            else //player rotates randomly without else statement more testing needed
            {
                anim.SetFloat("Speed", 0);
                rb.velocity = Vector3.zero;
                //rotates player to look at target location
                Quaternion targetRotation = Quaternion.LookRotation(towards);
                trans.rotation = Quaternion.Lerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }



        } else
        {
            anim.SetFloat("Speed", 0);
        }
    }
}

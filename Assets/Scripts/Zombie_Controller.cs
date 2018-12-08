using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Controller : MonoBehaviour {

    [SerializeField] private Transform trans;
    [SerializeField] private Rigidbody rb;

    private Transform playerTrans;
    private Rigidbody playerRb;

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

    private ZombieHealth health;

    private float range;

    private Animator anim;

    private GameObject player;

    public AudioClip punchClip;

    public AudioClip swordClip;

    private new AudioSource audio;
    RaycastHit hit;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        maxSpeed = 20f;
        radiusOfSat = 1.5f;
        range = 20f;
        turnSpeed = 5f;
        targetPoint = Vector3.zero;
        endpoint = trans.position;
        player = GameObject.FindGameObjectWithTag("Player");
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        health = GetComponent<ZombieHealth>();
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        movement();
        int damageToDeal = 0;
        //Detects to see if Player hits the Zombie
        if(Physics.Raycast(trans.position, trans.forward, out hit, 100))
        {
            
            if(hit.collider.tag == "Player")
            {
                //Damage if punching
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    damageToDeal = 5;
                    StartCoroutine(soundDelay(punchClip));
                    print("zombie is hurt");
                }
                //Damage if slashing
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    damageToDeal = 30;
                    StartCoroutine(soundDelay(swordClip));
                    print("zombie is hurt30");
                }
            }

            health.TakeDamage(damageToDeal);
        }

        
	}

    private void movement()
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
    }

   
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            anim.SetTrigger("Collision");
        }
    }

    private IEnumerator soundDelay(AudioClip clip)
    {
        yield return new WaitForSeconds(0.5f);

        audio.PlayOneShot(clip);
    }
}

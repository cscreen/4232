using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Character_Controller : MonoBehaviour {
	[HideInInspector] public GameObject controller;
	[HideInInspector] public GlobalVars target;
	[HideInInspector] public float speed = 13.0F;
	public float jumpVelocity = 550.0f;
	public float maxSlope = 60;
	private bool grounded = false;
	Rigidbody rb;

	void Start () {
		//Turns off cursor on screen and locks inside game window
		Cursor.lockState = CursorLockMode.Locked;
		rb = gameObject.GetComponent<Rigidbody> ();
		controller = GameObject.Find ("GameController");
		target = controller.GetComponent<GlobalVars> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Movement
		float translation = Input.GetAxis ("Vertical") * speed;
		float straffe = Input.GetAxis ("Horizontal") * speed;
		translation *= Time.deltaTime;
		straffe *= Time.deltaTime;
		transform.Translate (straffe, 0, translation);

		if (Input.GetButtonDown ("Jump") && grounded) {
			rb.AddForce (0, jumpVelocity, 0);
		}


	}
	// Death
	void OnCollisionEnter(Collision collision){
		if (collision.transform.tag == "Restart") {
			int scene = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadScene(scene, LoadSceneMode.Single);
		}

		// Finish Level
		if (collision.transform.tag == "Finish" && target.targetCount == 0) {
			StartCoroutine (ExecuteAfterTime (1));
		}
	}
	 //Detect if you on the ground to jump
	 void OnCollisionStay (Collision collision){
		foreach (ContactPoint contact in collision.contacts){

			if (Vector3.Angle(contact.normal, Vector3.up) < maxSlope){
				print ("Can Jump");
				grounded = true;
				speed = 13.0f;
			}
		}
	}

	void OnCollisionExit (Collision collision){
		print("Can't Jump");
		grounded = false;
		speed = 15.0f;
	}

	IEnumerator ExecuteAfterTime(float time){
		yield return new WaitForSeconds (time);
		int scene = SceneManager.GetActiveScene ().buildIndex;
		print (SceneManager.GetActiveScene ());
		if (scene < 2) {
			scene += 1;
		} else {
			scene = 0;
			Cursor.lockState = CursorLockMode.None;
			SceneManager.LoadScene(scene, LoadSceneMode.Single); // Retrun main menu on last level
		}
		SceneManager.LoadScene(scene, LoadSceneMode.Single); // Move to next level unless on last level
	}

}

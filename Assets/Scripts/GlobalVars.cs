using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalVars : MonoBehaviour {
	[HideInInspector] public GameObject[] getTargets;
	[HideInInspector] public int targetCount;
	public Text countText;
	// Use this for initialization
	void Start () {
		//getTargets = GameObject.FindGameObjectsWithTag ("Target");
		//targetCount = getTargets.Length;
		//countText.text = targetCount.ToString ();
	}
	
	// Update is called once per frame
	void Update () {

	}
}

using UnityEngine;


public class PauseSingle : MonoBehaviour {

	public static PauseSingle Instance { get; set; }

	public bool _enable = true;  // Stops player from firing while paused

	void Awake(){
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

}

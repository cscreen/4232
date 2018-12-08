using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Script : MonoBehaviour {


    public AudioClip footStep;
    AudioSource footStepSource;

    private void Start()
    {
        footStepSource = GetComponent<AudioSource>();
    }
    void footStepSound()
    {
        footStepSource.PlayOneShot(footStep);
    }

}

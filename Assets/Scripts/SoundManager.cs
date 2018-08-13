using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static AudioClip shootingSound;
    static AudioSource audioSrc;

	// Use this for initialization
	void Start () {
        shootingSound = Resources.Load<AudioClip>("shot1");

        audioSrc = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void PlayShootingSound()
    {
        audioSrc.PlayOneShot(shootingSound);
    }
}

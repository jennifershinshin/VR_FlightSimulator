using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphere : MonoBehaviour {
    // Use this for initialization
    public float targetTime = 5.0f;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(GetComponent<Renderer>().enabled == false)
        {
            targetTime -= Time.deltaTime;
            if(targetTime <= 0.0f)
            {
                GetComponent<Renderer>().enabled = true;
            }
        }
        else
        {
            targetTime = 5.0f;
        }
	}
}

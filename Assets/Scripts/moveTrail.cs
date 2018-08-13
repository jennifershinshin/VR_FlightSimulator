using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTrail : MonoBehaviour {

    public int moveSpeed = 500;
	
	// Update is called once per frame
	void Update () {
        transform.Translate(-Vector3.right * Time.deltaTime * moveSpeed);
        Destroy(gameObject, 5);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "sphere")
        {
            other.GetComponent<Renderer>().enabled = false;

        }
    }
}

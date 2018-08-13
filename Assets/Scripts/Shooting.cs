using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

    public float fireRate = 0;
    public LayerMask whatToHit;

    public Transform BulletTrailPrefab;

    float timeToFire = 0;
    Transform firePoint;

	// Use this for initialization
	void Awake () {
        firePoint = transform.Find("SPARTA PLANE BODY/SPARTA CANNON ANIMATED/SPARTA CANNON ARMATURE");
        if(firePoint == null)
        {
            Debug.LogError("No firepoint");
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(fireRate == 0)
        {
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                Shoot();
            }
        }
        else
        {
            if ((OVRInput.GetDown(OVRInput.Button.One)) && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
	}

    void Shoot()
    {
        Debug.Log("shooting");
        SoundManager.PlayShootingSound();
        Vector3 targetPosition = new Vector3(transform.position.x - 100, transform.position.y, transform.position.z);
        Vector3 firePointPosition = new Vector3(firePoint.position.x, firePoint.position.y, firePoint.position.z);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, targetPosition - firePointPosition, 50, whatToHit);
        Effect();
        Debug.DrawLine(firePointPosition, (targetPosition - firePointPosition) * 100, Color.cyan);
        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
        }
    }

    void Effect()
    {
        Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
    }
}

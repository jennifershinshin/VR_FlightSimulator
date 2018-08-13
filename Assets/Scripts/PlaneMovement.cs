using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    Rigidbody p_rigidBody;

    public float speed = 0;
    public float maxSpeed = 10;
    public float acceleration = 10;
    public float deceleration = 10;

    float tiltAngle = 90.0f;
    float tiltAroundX;
    float tiltAroundZ;
    float tiltAroundY;

    float changeY;

    // Use this for initialization
    void Start()
    {
        p_rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        //Debug.Log(input.x + ", " + input.y);
        Vector2 input2 = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        //Debug.Log(input2.x + ", " + input2.y);

        /* ACCELERATE, DECELLERATE, YAW */
        if (input.y > .7)
        {
            p_rigidBody.AddForce(transform.right * -200);
        }
        if (input.x < -.7)
        {
            transform.Rotate(transform.up, Time.deltaTime * -30);
        }
        if (input.y < -.7)
        {
            p_rigidBody.AddForce(transform.right * 30);
        }
        if (input.x > .7)
        {
            transform.Rotate(transform.up, Time.deltaTime * 30);
        }

        /* ROLL */
        if (input2.x > 0 && input2.y < .5 && input2.y > -.5)
        {
            transform.RotateAround(transform.position, transform.right, Time.deltaTime * 30f);
        }

        if (input2.x < 0 && input2.y < .5 && input2.y > -.5)
        {
            transform.RotateAround(transform.position, -transform.right, Time.deltaTime * 30f);
        }

        /* PITCH */
        if (input2.y > .5)
        {
            transform.RotateAround(transform.position, transform.forward, Time.deltaTime * -30f);
        }

        if (input2.y < -.5)
        {
            transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 30f);
        }
        p_rigidBody.velocity = p_rigidBody.velocity * 0.98f;

    }
}

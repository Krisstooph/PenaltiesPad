using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public GameObject ball;
    public float power = 9.0f;
    private Rigidbody ballRb;

    void Start()
    {
        ballRb = ball.GetComponentInChildren<Rigidbody>();
    }

    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
/*
        if (Input.GetButtonDown("Fire2"))
        {
            ballRb.AddForce(transform.forward * power, ForceMode.Impulse);
        }
*/
        if (Input.GetAxis("Horizontal") > 0.0f && Input.GetAxis("Vertical") >= 0.0f)
        {
            Debug.Log("Right top corner shot!");
        }

        if (Input.GetAxis("Horizontal") > 0.0f && Input.GetAxis("Vertical") < 0.0f)
        {
            Debug.Log("Right bottom corner shot!");
        }

        if (Input.GetAxis("Horizontal") < 0.0f && Input.GetAxis("Vertical") >= 0.0f)
        {
            Debug.Log("Left top corner shot!");
        }

        if (Input.GetAxis("Horizontal") < 0.0f && Input.GetAxis("Vertical") < 0.0f)
        {
            Debug.Log("Left bottom corner shot!");
        }
        
        if (Input.GetAxis("Horizontal") == 0.0f && Input.GetAxis("Vertical") > 0.0f)
        {
            Debug.Log("Center top shot!");
        }

        if (Input.GetAxis("Horizontal") == 0.0f && Input.GetAxis("Vertical") < 0.0f)
        {
            Debug.Log("Center bottom shot!");
        }

        //Debug.Log("X Axis: " + Input.GetAxis("Horizontal"));
        //Debug.Log("Y Axis: " + Input.GetAxis("Vertical"));
    }
}

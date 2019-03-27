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
        if (Input.GetButtonDown("Fire2"))
        {
            ballRb.AddForce(transform.forward * power, ForceMode.Impulse);
        }
    }
}

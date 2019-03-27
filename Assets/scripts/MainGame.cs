using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    private static String fireButton = "Fire2";
    private static float minShotForce = 12.0f;
    private static float maxGoalShotForce = 15.0f;
    private static float maxShotForce = 20.0f;
    private static float xShotFactor = 0.43f;
    private static float yShotFactor = 0.43f;

    public GameObject ball;
    public Vector3 xyz = new Vector3(0.43f, 0.35f, 1.0f);
    public float shotForceTMP = 15.0f;
    private Vector3 ballStartPosition;
    private Rigidbody ballRb;
    private float shotForce = minShotForce;
    
    void Start()
    {
        ballRb = ball.GetComponent<Rigidbody>();
        ballStartPosition = ball.gameObject.transform.position;
    }

    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        float missedShot = 0.0f;

        if (Input.GetButtonDown("Jump"))
        {
            ball.gameObject.transform.position = ballStartPosition;
            ballRb.velocity = Vector3.zero;
            ballRb.rotation = Quaternion.identity;
            shotForce = minShotForce;
        }

        if (Input.GetButton(fireButton))
        {
            shotForce += 0.15f;

            if (shotForce > maxShotForce)
                shotForce = maxShotForce;
        }

        if (Input.GetButtonUp(fireButton))
        {
            Vector3 shotVector = transform.forward;

            if (shotForce > maxGoalShotForce)
            {
                missedShot = (shotForce - maxGoalShotForce) / 10.0f;

                if (missedShot < 0.1f)
                    missedShot = 0.1f;

                Debug.Log("MISSED SHOT!");
            }

            if (Input.GetAxis("Horizontal") > 0.0f && Input.GetAxis("Vertical") >= 0.0f)
                shotVector = new Vector3(xShotFactor + missedShot, yShotFactor + missedShot, 1.0f);

            if (Input.GetAxis("Horizontal") > 0.0f && Input.GetAxis("Vertical") < 0.0f)
                shotVector = new Vector3(xShotFactor + missedShot, 0, 1.0f);

            if (Input.GetAxis("Horizontal") < 0.0f && Input.GetAxis("Vertical") >= 0.0f)
                shotVector = new Vector3(-xShotFactor - missedShot, yShotFactor + missedShot, 1.0f);

            if (Input.GetAxis("Horizontal") < 0.0f && Input.GetAxis("Vertical") < 0.0f)
                shotVector = new Vector3(-xShotFactor - missedShot, 0, 1.0f);

            if (Input.GetAxis("Horizontal") == 0.0f && Input.GetAxis("Vertical") > 0.0f)
                shotVector = new Vector3(0, yShotFactor + missedShot, 1.0f);

            if (Input.GetAxis("Horizontal") == 0.0f && Input.GetAxis("Vertical") < 0.0f)
                shotVector = new Vector3(0, 0, 1.0f);

            ballRb.AddForce(shotVector * shotForce, ForceMode.Impulse);
        }
    }
}

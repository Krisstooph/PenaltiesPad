using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:
// * Correct reset behaviour: ball should not rotate and move
public class MainGame : MonoBehaviour
{
    private static float minShotForce = 12.0f;
    private static float maxGoalShotForce = 15.0f;
    private static float maxShotForce = 20.0f;
    private static float xShotFactor = 0.43f;
    private static float yShotFactor = 0.43f;

    public GameObject ball;
    private Vector3 ballStartPosition;
    private Rigidbody ballRb;
    private float shotForce = minShotForce;
    private bool joy1Shooting = true;

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

        if (InputMgr.IsAnyResetButtonDown())
        {
            ball.gameObject.transform.position = ballStartPosition;
            ballRb.velocity = Vector3.zero;
            ballRb.rotation = Quaternion.identity;
            shotForce = minShotForce;
            joy1Shooting = !joy1Shooting;
        }

        if (InputMgr.IsShotButtonHeld(joy1Shooting))
        {
            shotForce += 0.15f;

            if (shotForce > maxShotForce)
                shotForce = maxShotForce;
        }

        if (InputMgr.IsShotButtonUp(joy1Shooting))
        {
            Vector3 shotVector = transform.forward;
            String footballerHorizontal;
            String footballerVertical;
            String goalkeeperHorizontal;
            String goalkeeperVertical;

            if (shotForce > maxGoalShotForce)
            {
                missedShot = (shotForce - maxGoalShotForce) / 10.0f;

                if (missedShot < 0.1f)
                    missedShot = 0.1f;
            }

            if (joy1Shooting)
            {
                footballerHorizontal = InputMgr.Joy1Horizontal;
                footballerVertical = InputMgr.Joy1Vertical;
                goalkeeperHorizontal = InputMgr.Joy2Horizontal;
                goalkeeperVertical = InputMgr.Joy2Vertical;
            }
            else
            {
                footballerHorizontal = InputMgr.Joy2Horizontal;
                footballerVertical = InputMgr.Joy2Vertical;
                goalkeeperHorizontal = InputMgr.Joy1Horizontal;
                goalkeeperVertical = InputMgr.Joy1Vertical;
            }

            if (Input.GetAxis(footballerHorizontal) > 0.0f && InputMgr.GetProperVerticalValue(footballerVertical) >= 0.0f)
                shotVector = new Vector3(xShotFactor + missedShot, yShotFactor + missedShot, 1.0f);

            if (Input.GetAxis(footballerHorizontal) > 0.0f && InputMgr.GetProperVerticalValue(footballerVertical) < 0.0f)
                shotVector = new Vector3(xShotFactor + missedShot, 0, 1.0f);

            if (Input.GetAxis(footballerHorizontal) < 0.0f && InputMgr.GetProperVerticalValue(footballerVertical) >= 0.0f)
                shotVector = new Vector3(-xShotFactor - missedShot, yShotFactor + missedShot, 1.0f);

            if (Input.GetAxis(footballerHorizontal) < 0.0f && InputMgr.GetProperVerticalValue(footballerVertical) < 0.0f)
                shotVector = new Vector3(-xShotFactor - missedShot, 0, 1.0f);

            if (Input.GetAxis(footballerHorizontal) == 0.0f && InputMgr.GetProperVerticalValue(footballerVertical) > 0.0f)
                shotVector = new Vector3(0, yShotFactor + missedShot, 1.0f);

            if (Input.GetAxis(footballerHorizontal) == 0.0f && InputMgr.GetProperVerticalValue(footballerVertical) < 0.0f)
                shotVector = new Vector3(0, 0, 1.0f);

            ballRb.AddForce(shotVector * shotForce, ForceMode.Impulse);
        }
    }


}

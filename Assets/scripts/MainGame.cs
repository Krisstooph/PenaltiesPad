using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    private static String fireButton = "Fire2";
    private static String joy1ButtonShot = "Joy1_Shot";
    private static String joy1ButtonOk = "Joy1_Ok";
    private static String joy1ButtonBack = "Joy1_Back";
    private static String joy1ButtonReset = "Joy1_Reset";
    private static String joy1Horizontal = "Joy1_Horizontal";
    private static String joy1Vertical = "Joy1_Vertical";
    private static String joy2ButtonShot = "Joy2_Shot";
    private static String joy2ButtonOk = "Joy2_Ok";
    private static String joy2ButtonBack = "Joy2_Back";
    private static String joy2ButtonReset = "Joy2_Reset";
    private static String joy2Horizontal = "Joy2_Horizontal";
    private static String joy2Vertical = "Joy2_Vertical";
    private static float minShotForce = 12.0f;
    private static float maxGoalShotForce = 15.0f;
    private static float maxShotForce = 20.0f;
    private static float xShotFactor = 0.43f;
    private static float yShotFactor = 0.43f;

    public GameObject ball;
    public bool mantaJoy = true;
    private Vector3 ballStartPosition;
    private Rigidbody ballRb;
    private float shotForce = minShotForce;
    private bool joy1Shooting = true;

    void Start()
    {
        ballRb = ball.GetComponent<Rigidbody>();
        ballStartPosition = ball.gameObject.transform.position;

        String[] names = Input.GetJoystickNames();

        foreach (String name in names)
            Debug.Log(name);
    }

    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        float missedShot = 0.0f;

        if (IsAnyResetButtonDown())
        {
            ball.gameObject.transform.position = ballStartPosition;
            ballRb.velocity = Vector3.zero;
            ballRb.rotation = Quaternion.identity;
            shotForce = minShotForce;
            joy1Shooting = !joy1Shooting;
        }

        if (IsShotButtonHeld())
        {
            shotForce += 0.15f;

            if (shotForce > maxShotForce)
                shotForce = maxShotForce;
        }

        if (IsShotButtonUp())
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
                footballerHorizontal = joy1Horizontal;
                footballerVertical = joy1Vertical;
                goalkeeperHorizontal = joy2Horizontal;
                goalkeeperVertical = joy2Vertical;
            }
            else
            {
                footballerHorizontal = joy2Horizontal;
                footballerVertical = joy2Vertical;
                goalkeeperHorizontal = joy1Horizontal;
                goalkeeperVertical = joy1Vertical;
            }

            if (Input.GetAxis(footballerHorizontal) > 0.0f && GetProperVerticalValue(footballerVertical) >= 0.0f)
                shotVector = new Vector3(xShotFactor + missedShot, yShotFactor + missedShot, 1.0f);

            if (Input.GetAxis(footballerHorizontal) > 0.0f && GetProperVerticalValue(footballerVertical) < 0.0f)
                shotVector = new Vector3(xShotFactor + missedShot, 0, 1.0f);

            if (Input.GetAxis(footballerHorizontal) < 0.0f && GetProperVerticalValue(footballerVertical) >= 0.0f)
                shotVector = new Vector3(-xShotFactor - missedShot, yShotFactor + missedShot, 1.0f);

            if (Input.GetAxis(footballerHorizontal) < 0.0f && GetProperVerticalValue(footballerVertical) < 0.0f)
                shotVector = new Vector3(-xShotFactor - missedShot, 0, 1.0f);

            if (Input.GetAxis(footballerHorizontal) == 0.0f && GetProperVerticalValue(footballerVertical) > 0.0f)
                shotVector = new Vector3(0, yShotFactor + missedShot, 1.0f);

            if (Input.GetAxis(footballerHorizontal) == 0.0f && GetProperVerticalValue(footballerVertical) < 0.0f)
                shotVector = new Vector3(0, 0, 1.0f);

            ballRb.AddForce(shotVector * shotForce, ForceMode.Impulse);
        }
    }

    private bool IsShotButtonHeld()
    {
        if (joy1Shooting)
            return Input.GetButton(joy1ButtonShot);
        else
            return Input.GetButton(joy2ButtonShot);
    }

    private bool IsShotButtonUp()
    {
        if (joy1Shooting)
            return Input.GetButtonUp(joy1ButtonShot);
        else
            return Input.GetButtonUp(joy2ButtonShot);
    }

    private bool IsAnyOkButtonDown()
    {
        // MANTA: OK (xbox360) = BACK (manta)
        return Input.GetButtonDown(joy1ButtonOk) || (!mantaJoy && Input.GetButtonDown(joy2ButtonOk)) || (mantaJoy && Input.GetButtonDown(joy2ButtonBack));
    }

    private bool IsAnyBackButtonDown()
    {
        // MANTA: BACK (xbox360) = RESET (manta)
        return Input.GetButtonDown(joy1ButtonBack) || (!mantaJoy && Input.GetButtonDown(joy2ButtonBack)) || (mantaJoy && Input.GetButtonDown(joy2ButtonReset));
    }

    private bool IsAnyResetButtonDown()
    {
        // MANTA: RESET (xbox360) = OK (manta)
        return Input.GetButtonDown(joy1ButtonReset) || (!mantaJoy && Input.GetButtonDown(joy2ButtonReset)) || (mantaJoy && Input.GetButtonDown(joy2ButtonOk));
    }

    private float GetProperVerticalValue(String verticalAxis)
    {
        if (mantaJoy && verticalAxis.Equals(joy2Vertical))
            return -Input.GetAxis(verticalAxis);    // Manta has inverted Y-axis

        return Input.GetAxis(verticalAxis);
    }
}

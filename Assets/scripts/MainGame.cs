using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:
// * Correct reset behaviour: ball and goalkeeper should not rotate and move
// * [UNITY] Add more colliders behind the goal - ball often flies through it
public class MainGame : MonoBehaviour
{
    private static float minShotForce = 12.0f;
    private static float maxGoalShotForce = 15.0f;
    private static float maxShotForce = 20.0f;
    private static float xShotFactor = 0.43f;
    private static float yShotFactor = 0.35f;

    public GameObject ball;
    public GameObject goalkeeper;
    public GameObject invisibleWall;
    private Vector3 ballStartPosition;
    private Vector3 goalkeeperStartPosition;
    private Rigidbody ballRb;
    private Rigidbody goalkeeperRb;
    private BoxCollider wallCollider;
    private bool joy1Shooting = true;
    private float shotForce = minShotForce;

    void Start()
    {
        ballRb = ball.GetComponent<Rigidbody>();
        goalkeeperRb = goalkeeper.GetComponent<Rigidbody>();
        wallCollider = invisibleWall.GetComponent<BoxCollider>();
        ballStartPosition = ball.gameObject.transform.position;
        goalkeeperStartPosition = goalkeeper.gameObject.transform.position;
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
            ball.gameObject.transform.SetPositionAndRotation(ballStartPosition, Quaternion.identity);
            ballRb.velocity = Vector3.zero;
            goalkeeper.gameObject.transform.SetPositionAndRotation(goalkeeperStartPosition, Quaternion.identity);
            goalkeeper.gameObject.transform.Rotate(Vector3.up, 180.0f);
            shotForce = minShotForce;
            wallCollider.enabled = false;
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
            Vector3 shotVector;
            Vector3 saveVector;
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

            shotVector = FootballerAction(footballerHorizontal, footballerVertical, missedShot);
            ballRb.AddForce(shotVector * shotForce, ForceMode.Impulse);
            saveVector = GoalkeeperAction(goalkeeperHorizontal, goalkeeperVertical);
            goalkeeperRb.AddForce(saveVector * 250.0f, ForceMode.Impulse);
        }
    }

    private Vector3 FootballerAction(String footballerHorizontal, String footballerVertical, float missedShot)
    {
        Vector3 shotVector = Vector3.forward;

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

        return shotVector;
    }

    private Vector3 GoalkeeperAction(String goalkeeperHorizontal, String goalkeeperVertical)
    {
        Vector3 saveVector = Vector3.zero;

        if (Input.GetAxis(goalkeeperHorizontal) > 0.0f && InputMgr.GetProperVerticalValue(goalkeeperVertical) >= 0.0f)
        {
            saveVector = new Vector3(1.5f, 1.3f, 0);
            wallCollider.enabled = true;
        }

        if (Input.GetAxis(goalkeeperHorizontal) > 0.0f && InputMgr.GetProperVerticalValue(goalkeeperVertical) < 0.0f)
        {
            saveVector = new Vector3(2, 0.3f, 0);
            wallCollider.enabled = true;
        }

        if (Input.GetAxis(goalkeeperHorizontal) < 0.0f && InputMgr.GetProperVerticalValue(goalkeeperVertical) >= 0.0f)
        {
            saveVector = new Vector3(-1.5f, 1.3f, 0);
            wallCollider.enabled = true;
        }
        if (Input.GetAxis(goalkeeperHorizontal) < 0.0f && InputMgr.GetProperVerticalValue(goalkeeperVertical) < 0.0f)
        {
            saveVector = new Vector3(-2, 0.3f, 0);
            wallCollider.enabled = true;
        }

        if (Input.GetAxis(goalkeeperHorizontal) == 0.0f && InputMgr.GetProperVerticalValue(goalkeeperVertical) > 0.0f)
            saveVector = Vector3.up;

        return saveVector;
    }
}

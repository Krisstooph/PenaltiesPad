using UnityEngine;
using System.Collections;
using System;

public class InputMgr : MonoBehaviour
{
    public static bool mantaJoy = true;
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

    public static string Joy1Horizontal { get => joy1Horizontal; }
    public static string Joy1Vertical { get => joy1Vertical; }
    public static string Joy2Horizontal { get => joy2Horizontal; }
    public static string Joy2Vertical { get => joy2Vertical; }

    private void Start()
    {
        String[] names = Input.GetJoystickNames();

        foreach (String name in names)
            Debug.Log(name);
    }

    public static bool IsShotButtonHeld(bool joy1Shooting)
    {
        if (joy1Shooting)
            return Input.GetButton(joy1ButtonShot);
        else
            return Input.GetButton(joy2ButtonShot);
    }

    public static bool IsShotButtonUp(bool joy1Shooting)
    {
        if (joy1Shooting)
            return Input.GetButtonUp(joy1ButtonShot);
        else
            return Input.GetButtonUp(joy2ButtonShot);
    }

    public static bool IsAnyOkButtonDown()
    {
        // OK (xbox360) = BACK (manta)
        return Input.GetButtonDown(joy1ButtonOk) || (!mantaJoy && Input.GetButtonDown(joy2ButtonOk)) || (mantaJoy && Input.GetButtonDown(joy2ButtonBack));
    }

    public static bool IsAnyBackButtonDown()
    {
        // BACK (xbox360) = RESET (manta)
        return Input.GetButtonDown(joy1ButtonBack) || (!mantaJoy && Input.GetButtonDown(joy2ButtonBack)) || (mantaJoy && Input.GetButtonDown(joy2ButtonReset));
    }

    public static bool IsAnyResetButtonDown()
    {
        // RESET (xbox360) = OK (manta)
        return Input.GetButtonDown(joy1ButtonReset) || (!mantaJoy && Input.GetButtonDown(joy2ButtonReset)) || (mantaJoy && Input.GetButtonDown(joy2ButtonOk));
    }

    public static float GetProperVerticalValue(String verticalAxis)
    {
        if (mantaJoy && verticalAxis.Equals(joy2Vertical))
            return -Input.GetAxis(verticalAxis);    // Manta has inverted Y-axis

        return Input.GetAxis(verticalAxis);
    }
}

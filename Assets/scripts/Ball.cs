using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        
    }

    public bool IsGoal()
    {
        bool xInGoal = (transform.position.x > -2.44f) && (transform.position.x < 2.3f);
        bool yInGoal = (transform.position.y > 0.01f) && (transform.position.y < 1.4f);
        bool zInGoal = (transform.position.z > 3.62f) && (transform.position.z < 4.60f);

        return xInGoal && yInGoal && zInGoal;
    }
}

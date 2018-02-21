using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class ObjectDetectorScript : MonoBehaviour
{

    public float angle;
    private bool useAngle = true;

    public float strength;
    public int numObjects;

    void Start()
    {
        strength = 0;
        numObjects = 0;

        if (angle >= 360)
        {
            useAngle = false;
        }
    }

    void Update()
    {
        GameObject[] obstacles;

        if (useAngle)
        {
            obstacles = GetVisibleObstacle();
        }
        else
        {
            obstacles = GetAllObstacle();
            Console.Write("ajsd");
        }

        strength = 0;
        numObjects = obstacles.Length;

        foreach (GameObject obstacle in obstacles)
        {
            float r = obstacle.GetComponent<Rigidbody>().mass + 10;
            strength += 1.0f / ((transform.position - obstacle.transform.position).sqrMagnitude / r + 1);
            Debug.DrawLine(transform.position, obstacle.transform.position, Color.red);
        }

        if (numObjects > 0)
        {
            strength = strength / numObjects;
        }
    }

    // Get linear output value
    public float GetLinearOutput()
    {
        return strength;
    }

    // Get gaussian output value
    public virtual float GetGaussianOutput()
    {
        throw new NotImplementedException();
    }

    // Returns all "Light" tagged objects. The sensor angle is not taken into account.
    GameObject[] GetAllObstacle()
    {
        return GameObject.FindGameObjectsWithTag("Obstacle");
    }

    // Returns all "Light" tagged objects that are within the view angle of the Sensor. 
    // Only considers the angle over the y axis. Does not consider objects blocking the view.
    GameObject[] GetVisibleObstacle()
    {
        ArrayList visibleObstacles = new ArrayList();
        float halfAngle = angle / 2.0f;

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            Vector3 toVector = (obstacle.transform.position - transform.position);
            Vector3 forward = transform.forward;
            toVector.y = 0;
            forward.y = 0;
            float angleToTarget = Vector3.Angle(forward, toVector);

            if (angleToTarget <= halfAngle)
            {
                visibleObstacles.Add(obstacle);
            }
        }

        return (GameObject[])visibleObstacles.ToArray(typeof(GameObject));
    }


}

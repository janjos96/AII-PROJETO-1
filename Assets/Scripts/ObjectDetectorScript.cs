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
        GameObject[] obstacles; //array que guarda os obstaculos

        if (useAngle) //quando o angulo for menor que 360
        {
            obstacles = GetVisibleObstacle(); //o array so guarda os obstaculos visiveis neste angulo
        }
        else
        {
            obstacles = GetAllObstacle(); //quando o angulo for 360, o array guarda todos os obstaculos
            Console.Write("ajsd");
        }

        strength = 0;
        numObjects = obstacles.Length;

        foreach (GameObject obstacle in obstacles) //para cada obstaculo dentro do array
        {
            float r = obstacle.GetComponent<Rigidbody>().mass;
            strength += 1.0f / ((transform.position - obstacle.transform.position).sqrMagnitude / r + 1);
            Debug.DrawLine(transform.position, obstacle.transform.position, Color.red); //desenha linhas que mostram a detecao dos obstaculos
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

    // Returns all "Obstacle" tagged objects. The sensor angle is not taken into account.
    GameObject[] GetAllObstacle()
    {
        return GameObject.FindGameObjectsWithTag("Obstacle");
    }

    // Returns all "Obstacle" tagged objects that are within the view angle of the Sensor. 
    // Only considers the angle over the y axis. Does not consider objects blocking the view.
    GameObject[] GetVisibleObstacle()
    {
        ArrayList visibleObstacles = new ArrayList();
        float halfAngle = angle / 2.0f;

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles) 
        {
            Vector3 toVector = (obstacle.transform.position - transform.position); //distancia entre o sensor e o obstaculo
			Vector3 forward = transform.forward;
            toVector.y = 0;
            forward.y = 0;
            float angleToTarget = Vector3.Angle(forward, toVector); //angulo entre o vetor forward e toVector

            if (angleToTarget <= halfAngle) //se o angulo entre os vetores anteriores for menor que metade do angulo
											//de captacao dos sensores, o obstaculo fica guardado no array de obstaculos visiveis
            {
                visibleObstacles.Add(obstacle);
            }
        }

        return (GameObject[])visibleObstacles.ToArray(typeof(GameObject)); //devolve todos os GameObject em forma de array
    }


}

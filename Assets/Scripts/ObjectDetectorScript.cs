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
            obstacles = GetVisibleObstacle(); //o array contém os obstaculos visiveis neste angulo
        }
        else
        {
            obstacles = GetAllObstacle(); //quando o angulo for 360, o array contém todos os obstaculos
        }

        strength = 0;
        numObjects = obstacles.Length; // devolve número de obstaculos encontrados num instante

        foreach (GameObject obstacle in obstacles) //para cada obstaculo dentro do array
        {
            float r = obstacle.GetComponent<Rigidbody>().mass; //cria um variavél "r" que contém a massa do obstaculo
            strength += 1.0f / ((transform.position - obstacle.transform.position).sqrMagnitude / r + 0.5f); //sqrMagnitude devolve o valor da distancia ao obstaculo e divide o valor pelo centro de massa do obstaculo
            Debug.DrawLine(transform.position, obstacle.transform.position, Color.red); //desenha linhas que mostram a detecao dos obstaculos

        }

        if (numObjects > 0) 
        {
            strength = strength / numObjects; //divide a forca a ser exercicida nas rodas pelo numero de objectos ativos nos sensores
        }
    }

    // Get linear output value
    // Existem limites e por isso, se a força for menor que 0.2 o sensor despreza essa força e não a passa às rodas assim como se for maio que 0.8 este valor não aumenta
    public float GetLinearOutput()
    {
        return strength;
    }

    // Get gaussian output value
    public virtual float GetGaussianOutput()
    {
        float desvio = 0.12f;
        float media = 0.5f;

        //float value = ((1.0f / (desvio * Mathf.Sqrt(2.0f * Mathf.PI))) * (Mathf.Exp((-(Mathf.Pow(strength - media, 2.0f)) / (2.0f * Mathf.Pow(desvio, 2.0f))))));

        float value = 1 * Mathf.Exp(-(Mathf.Pow(strength - media, 2)) / (2 * Mathf.Pow(desvio, 2)));

        return value;
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
        float halfAngle = angle / 2.0f; //dividir o angulo pois cada sensor tem um angulo próprio

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles) 
        {
            Vector3 toVector = (obstacle.transform.position - transform.position); //vetor entre o sensor e o obstaculo
            Vector3 forward = transform.forward; //devolve o vetor no eixo azul (z)
            toVector.y = 0;
            forward.y = 0;
            float angleToTarget = Vector3.Angle(forward, toVector); //angulo entre o vetor forward e toVector, neste caso entre o sensor e o obstaculo

            if (angleToTarget <= halfAngle) //se o angulo entre os vetores anteriores for menor que metade do angulo
											//de captacao dos sensores, o obstaculo fica guardado no array de obstaculos visiveis
            {
                visibleObstacles.Add(obstacle);
            }
        }

        return (GameObject[])visibleObstacles.ToArray(typeof(GameObject)); //devolve todos os GameObject validados em forma de array
    }


}

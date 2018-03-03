using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class LightDetectorScript : MonoBehaviour {

	public float angle;
	private bool useAngle = true;

	public float strength;
	public int numObjects;

    public bool inverse;

    public float strengthLinear;
    public float strengthGaussiana;

    public float desvioPadrao;
    public float media;

    public float maxLimiar;
    public float minLimiar;
    public float maxLimite;
    public float minLimite;

	void Start () {
		strength = 0;
		numObjects = 0;

		if (angle >= 360) {
			useAngle = false;
		}
	}

	void Update () {
        GameObject[] lights; //array que guarda as luzes

        if (useAngle) { //quando o angulo for menor que 360
            lights = GetVisibleLights (); //o array contém as luzes visiveis neste angulo
		} else {
            lights = GetAllLights (); //quando o angulo for 360, o array contém todas as luzes
		}

		strength = 0;
        numObjects = lights.Length; // devolve número de luzes encontradas num instante
	
		foreach (GameObject light in lights) { //para cada luz dentro do array
            float r = light.GetComponent<Light> ().range; //cria um variavél "r" que contém o alcance da luz
            strength += 1.0f / ((transform.position - light.transform.position).sqrMagnitude / r + 1); //sqrMagnitude devolve o valor da distancia à fonte de luz e divide o valor pelo alcance desta
            Debug.DrawLine(transform.position, light.transform.position, Color.blue); //desenha linhas que mostram a detecao das luzes
		}

		if (numObjects > 0) {
            strength = strength / numObjects; //divide a forca a ser exercicida nas rodas pelo numero de objectos ativos nos sensores (luzes)
		}
	}

	// Get linear output value
    // Existem limites e por isso, se a força for menor que 0.1 o sensor despreza essa força e não a passa às rodas assim como se for maio que 0.8 este valor não aumenta
	public float GetLinearOutput()
	{
        /*if (strength <= 0.25f) { strength = 0; }
        if (strength >= 0.75f) { strength = 0; }

        float output = strength;

        if (output == 0.0f) { output = 0.05f; }
        if (output >= 0.6f) { output = 0.6f; }*/

        if (inverse){
            strengthLinear = -strength + 1;
        } else {
            strengthLinear = strength;
        }

		return strengthLinear;
	}

    // Get gaussian output value
    public virtual float GetGaussianOutput()
    {

        //float value = ((1.0f / (desvio * Mathf.Sqrt(2.0f * Mathf.PI))) * (Mathf.Exp((-(Mathf.Pow(strength - media, 2.0f)) / (2.0f * Mathf.Pow(desvio, 2.0f))))));

        if (inverse){
            strengthGaussiana = -(1 * Mathf.Exp(-(Mathf.Pow(strength - media, 2)) / (2 * Mathf.Pow(desvioPadrao, 2))))+1;
        } else {
            strengthGaussiana = 1 * Mathf.Exp(-(Mathf.Pow(strength - media, 2)) / (2 * Mathf.Pow(desvioPadrao, 2)));
        }

        return strengthGaussiana;

	}
		
	// Returns all "Light" tagged objects. The sensor angle is not taken into account.
	GameObject[] GetAllLights()
	{
		return GameObject.FindGameObjectsWithTag ("Light");
	}

	// Returns all "Light" tagged objects that are within the view angle of the Sensor. 
	// Only considers the angle over the y axis. Does not consider objects blocking the view.
	GameObject[] GetVisibleLights()
	{
		ArrayList visibleLights = new ArrayList();
        float halfAngle = angle / 2.0f; //dividir o angulo pois cada sensor tem um angulo próprio

		GameObject[] lights = GameObject.FindGameObjectsWithTag ("Light");

		foreach (GameObject light in lights) {
            Vector3 toVector = (light.transform.position - transform.position); //vetor entre o sensor e a fonte de luz
            Vector3 forward = transform.forward; // devolve o vetor no eixo azul (z)
			toVector.y = 0;
			forward.y = 0;
            float angleToTarget = Vector3.Angle (forward, toVector); //angulo entre o vetor forward e toVector, neste caso entre o sensor e a luz

            if (angleToTarget <= halfAngle) //se o angulo entre os vetores anteriores for menor que metade do angulo
                                            //de captacao dos sensores, a fonte de luz fica guardada no array de luzes visiveis
            { 
				visibleLights.Add (light);
			}
		}

        return (GameObject[])visibleLights.ToArray(typeof(GameObject)); //devolve todos os GameObject validados em forma de array
	}


}

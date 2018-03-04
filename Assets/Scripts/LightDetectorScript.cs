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
    public bool drawLines;

    public float strengthLinear;
    public float strengthGaussiana;

    public float desvioPadrao;
    public float media;

    public bool limiar;
    public bool limite;
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
            if(drawLines){
                Debug.DrawLine(transform.position, light.transform.position, Color.blue); //desenha linhas que mostram a detecao das luzes
            }
		}

		if (numObjects > 0) {
            strength = strength / numObjects; //divide a forca a ser exercicida nas rodas pelo numero de objectos ativos nos sensores (luzes)
		}
	}

	// Get linear output value
    // Get linar output não altera a função que devolve a strenght apenas devolve este valor
	public float GetLinearOutput()
	{
        // função invertida, caso seja ativada, simula o comportamento inverso no carrinho
        // se este se estiver a aproximar da luz, irá passar a fugir
        if (inverse){
            strengthLinear = -strength + 1;
        } else { // caso contrário nenhum calculo é efetuado
            strengthLinear = strength;
        }

        // caso se esteja a usar limiares e a boolean ativada
        if(limiar) {
            // se o valor da strength calculada acima for maior que o limiar superior ou
            // menor que o limiar inferior, a força não sofre alterações e é igual a 0
            if(strengthLinear < minLimiar || strengthLinear > maxLimiar) {
                strengthLinear = 0;
            }
        }

        // caso se esteja a usar limites e a boolean ativada
        if(limite) {
            // se o valor da força calculada acima for menor que o limite minimo, passa a assumir o valor do limite mínimo
            if(strengthLinear < minLimite){
                strengthLinear = minLimite;
            // se o valor da força calculada acima for maior que o limite máximo, passa a assumir o valor do limite máximo
            } else if (strengthLinear > maxLimite){
                strengthLinear = maxLimite;
            }
        }

		return strengthLinear;
	}

    // Get gaussian output value
    // Get gaussian output calcula o valor da strenght usando uma função gaussiana onde o desvio padrão e media são definidos nos sensores
    public virtual float GetGaussianOutput()
    {
        //Primeira função gaussiana usada, optamos por usar a funcão abaixo por ser mais facil controlar a altura da função
        //float value = ((1.0f / (desvio * Mathf.Sqrt(2.0f * Mathf.PI))) * (Mathf.Exp((-(Mathf.Pow(strength - media, 2.0f)) / (2.0f * Mathf.Pow(desvio, 2.0f))))));


        // função invertida, caso seja ativada, simula o comportamento inverso no carrinho
        // se este se estiver a aproximar da luz, irá passar a fugir
        if (inverse){
            //função invertida = -gaussiana+1
            strengthGaussiana = -(1 * Mathf.Exp(-(Mathf.Pow(6 - media, 2)) / (2 * Mathf.Pow(desvioPadrao, 2))))+1;
        } else {
            //função gaussiana usada para calcular a strenght, o primeiro número controla a altura da função
            //neste caso é um pois os valores de strenght estão sempre entre 0 e 1
            strengthGaussiana = 1 * Mathf.Exp(-(Mathf.Pow(strength - media, 2)) / (2 * Mathf.Pow(desvioPadrao, 2)));
        }

        // caso se esteja a usar limiares e a boolean ativada
        if (limiar){
            // se o valor da strength calculada acima for maior que o limiar superior ou
            // menor que o limiar inferior, a força não sofre alterações e é igual a 0
            if (strengthGaussiana < minLimiar || strengthGaussiana > maxLimiar){
                strengthGaussiana = 0;
            }
        }

        // caso se esteja a usar limites e a boolean ativada
        if (limite){
            // se o valor da força calculada acima for menor que o limite minimo, passa a assumir o valor do limite mínimo
            if (strengthGaussiana < minLimite){
                strengthGaussiana = minLimite;
            }
            // se o valor da força calculada acima for maior que o limite máximo, passa a assumir o valor do limite máximo
            else if (strengthGaussiana > maxLimite){
                strengthGaussiana = maxLimite;
            }
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

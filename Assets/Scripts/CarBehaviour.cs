using UnityEngine;
using System.Collections;

public class CarBehaviour : MonoBehaviour {
	
	public float MaxSpeed;
	public WheelCollider RR;
	public WheelCollider RL;
	public LightDetectorScript LeftLD;
	public LightDetectorScript RightLD;
    public ObjectDetectorScript LeftOD;
    public ObjectDetectorScript RightOD;

	private Rigidbody m_Rigidbody;
	public float m_LeftWheelSpeed;
	public float m_RightWheelSpeed;
	private float m_axleLength;

    public bool gaussian;

	void Start()
	{
		m_Rigidbody = GetComponent<Rigidbody> (); //componente para efetuar alterações no carro
		m_axleLength = (RR.transform.position - RL.transform.position).magnitude; //Magnitude do vetor entre a posição de cada roda
	}

	void FixedUpdate () {
		//Calculate forward movement
		float targetSpeed = (m_LeftWheelSpeed + m_RightWheelSpeed) / 2; //divide a velocidade de cada roda por dois para dar velocidade ao carro
        Vector3 movement = transform.forward * targetSpeed * Time.deltaTime; //vetor de momivento do carrinho, quanto este se vai deslocar (time.deltatime devolve o tempo da ultima frame)

		//Calculate turn degrees based on wheel speed
		float angVelocity = (m_LeftWheelSpeed - m_RightWheelSpeed) / m_axleLength * Mathf.Rad2Deg * Time.deltaTime;
		Quaternion turnRotation = Quaternion.Euler (0.0f, angVelocity, 0.0f); //devolve a rotação que vai rodar no eixo y com o valor de angVelocity

		//Apply to rigid body
		m_Rigidbody.MovePosition (m_Rigidbody.position + movement);
		m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation); 
	}
}

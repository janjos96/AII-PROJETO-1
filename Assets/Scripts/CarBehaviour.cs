using UnityEngine;
using System.Collections;

public class CarBehaviour : MonoBehaviour {
	
	public float MaxSpeed;
	public WheelCollider RR;
	public WheelCollider RL;
	public LightDetectorScript LeftLD;
	public LightDetectorScript RightLD;

	private Rigidbody m_Rigidbody;
	public float m_LeftWheelSpeed;
	public float m_RightWheelSpeed;
	private float m_axleLength;

	void Start()
	{
		m_Rigidbody = GetComponent<Rigidbody> ();
		m_axleLength = (RR.transform.position - RL.transform.position).magnitude;
	}

	void FixedUpdate () {
		//Calculate forward movement
		float targetSpeed = (m_LeftWheelSpeed + m_RightWheelSpeed) / 2;
		Vector3 movement = transform.forward * targetSpeed * Time.deltaTime;

		//Calculate turn degrees based on wheel speed
		float angVelocity = (m_LeftWheelSpeed - m_RightWheelSpeed) / m_axleLength * Mathf.Rad2Deg * Time.deltaTime;
		Quaternion turnRotation = Quaternion.Euler (0.0f, angVelocity, 0.0f);

		//Apply to rigid body
		m_Rigidbody.MovePosition (m_Rigidbody.position + movement);
		m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation); 
	}
}

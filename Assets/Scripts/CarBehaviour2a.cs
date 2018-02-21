using UnityEngine;
using System.Collections;

public class CarBehaviour2a : CarBehaviour {
	
	void Update()
	{
		//Read sensor values
        float leftSensor = LeftLD.GetLinearOutput () + LeftOD.GetLinearOutput();
        float rightSensor = RightLD.GetLinearOutput () + RightOD.GetLinearOutput();

		//Calculate target motor values
		m_LeftWheelSpeed = leftSensor * MaxSpeed;
		m_RightWheelSpeed = rightSensor * MaxSpeed;
	}
}

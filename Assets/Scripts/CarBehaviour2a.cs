using UnityEngine;
using System.Collections;

public class CarBehaviour2a : CarBehaviour {
	
	void Update()
	{

        float leftLightSensor;
        float rightLightSensor;

        if(gaussian){
            //Read light sensor values
            leftLightSensor = LeftLD.GetGaussianOutput();
            rightLightSensor = RightLD.GetGaussianOutput();
        } else {
            //Read light sensor values
            leftLightSensor = LeftLD.GetLinearOutput();
            rightLightSensor = RightLD.GetLinearOutput();
        }

        //Read obstacle sensor values
        float leftObstacleSensor = LeftOD.GetLinearOutput();
        float rightObstacleSensor = RightOD.GetLinearOutput();

        // Calculate target motor values
        // LightSensors estão ligados cruzados e ObstacleSensor estão ligados diretamente
		m_LeftWheelSpeed = (rightLightSensor + leftObstacleSensor) * MaxSpeed;
        m_RightWheelSpeed = (leftLightSensor + rightObstacleSensor) * MaxSpeed;
	}
}

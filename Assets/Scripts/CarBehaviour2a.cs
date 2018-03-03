using UnityEngine;
using System.Collections;

public class CarBehaviour2a : CarBehaviour {
	
	void Update()
	{

        float leftLightSensor;
        float rightLightSensor;

        if(gaussian){ // caso a boolean gaussiana esteja ativada a strenght das rodas irá ser calculada pela função gaussiana
            //Read light sensor values
            leftLightSensor = LeftLD.GetGaussianOutput();
            rightLightSensor = RightLD.GetGaussianOutput();
        } else { // caso a boolean gaussiana NÃO esteja ativada a strenght das rodas irá ser calculada linearmente
                 //sem sofrer alterações da funçao normal
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveWheel : MonoBehaviour
{
	public event Action<bool> OnGroundedChanged;

	[SerializeField] private Rigidbody m_RB;
	[SerializeField] private TankSO m_Data;
	[SerializeField] private Suspension[] m_SuspensionWheels;
	private int m_NumGroundedWheels;
	private float m_ForwardMaxSpeed;
	private float m_ReverseMaxSpeed;
	private bool m_Grounded;

	private float m_Acceleration;
	public void SetAcceleration(float amount)
	{
		m_Acceleration = amount;
	}

	public void Init(TankSO inData)
	{
		m_Data = inData;
		m_RB.mass = m_Data.Mass_Tons;
		m_ForwardMaxSpeed = 11.8f;
		m_ReverseMaxSpeed = 3.33f;
        foreach (Suspension suspensionWheel in m_SuspensionWheels)
        {
			suspensionWheel.OnGroundedChanged += Handle_WheelGroundedChanged;
        }
    }

	private void Handle_WheelGroundedChanged(bool newGrounded)
	{
		if (newGrounded == true)
			m_NumGroundedWheels++;

		if(newGrounded == false)
			m_NumGroundedWheels--;
	}

	private void FixedUpdate()
	{
		Debug.Log(m_NumGroundedWheels);
		//Debug.Log(m_Acceleration);
		//Debug.Log(m_RB.mass);
		float currentSpeed = m_RB.velocity.magnitude;
		if (m_Acceleration == 1)
		{
			Vector3 forward = transform.forward * m_Data.EngineData.HorsePower;
			m_RB.AddForce(forward);
			if (currentSpeed > m_ForwardMaxSpeed)
				m_RB.velocity = m_RB.velocity.normalized * m_ForwardMaxSpeed;
		}

		if (m_Acceleration == -1)
		{
            Vector3 reverse = -transform.forward * m_Data.EngineData.HorsePower;
            m_RB.AddForce(reverse);

			if(currentSpeed > m_ReverseMaxSpeed)
			{
				m_RB.velocity = m_RB.velocity.normalized * m_ReverseMaxSpeed;
			}
        }
	}
}
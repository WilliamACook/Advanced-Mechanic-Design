using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
	public event Action<bool> OnGroundedChanged; 

	[SerializeField] private Transform m_Wheel;
	[SerializeField] private Rigidbody m_RB;
	[SerializeField] private LayerMask m_LayerMask;

	[SerializeField] float springStrength;
	[SerializeField] float springDamper;

	private SuspensionSO m_Data;
	private float m_SpringSize = 0.3f;
	private bool m_Grounded;

	public void Init(SuspensionSO inData)
	{
		m_Data = inData;
		springStrength = m_Data.SuspensionStrength;
		springDamper = m_Data.SuspensionDamper;
	}

	public bool GetGrounded()
	{
		RaycastHit hit;

        bool grounded = Physics.Raycast(transform.position, -transform.up, out hit, m_SpringSize, m_LayerMask);
		if (grounded)
		{
			
			if(m_Grounded != grounded)
			{
				m_Grounded = grounded;
				OnGroundedChanged?.Invoke(m_Grounded);
				//Debug.Log("Tank is grounded");
			}
		}
		else
		{
			if(m_Grounded != grounded)
			{
				m_Grounded = grounded;
				OnGroundedChanged?.Invoke(m_Grounded);
				//Debug.Log("NotGrounded");
			}
				
		}

      
        return m_Grounded;
    }

	private void FixedUpdate()
	{
		//Vector3 direction = Vector3.down;
		//Vector3 localDir = transform.TransformDirection(direction);
		//bool grounded = GetGrounded();

		//Vector3 springVec = transform.position - transform.parent.position;
		//float susOffset = m_SpringSize - Vector3.Dot(springVec, localDir);

		////m_Wheel.localPosition = Vector3.up * (m_SpringSize - susOffset);

		//Vector3 worldVec = m_RB.GetPointVelocity(transform.position);
		//float susVel = Vector3.Dot(localDir, worldVec);
		//float susForce = (susOffset * stiffness) - (susVel * damping);
		//m_RB.AddForceAtPosition(localDir * susForce, worldVec);

		////float susForce = (susOffset * m_Data.SuspensionStrength) - (susVel * m_Data.SuspensionDamper);

		//pseudocode:
		//----------
		// - raycast; if no hit:
		//		- set "displacement" to the max length of suspension
		// - if hit:
		//		- if distance > max length.. set to max length
		//		- if distance <= max length then:
		//			- move the wheel to hit point
		//			- push the tank up .. force should be inversely proportional to distance between origin and hitpoint: (closer = more force)

		RaycastHit hit;

		bool hitFloor = Physics.Raycast(transform.position, -transform.up, out hit, m_SpringSize, m_LayerMask);
		float upwardForceMultiplier = 10f;
        //Debug.DrawRay(transform.position, -transform.up * m_SpringSize, Color.red, 0.1f);
        if (GetGrounded())
		{
			if(hitFloor)
			{				

				Vector3 springDir = m_Wheel.up;

				Vector3 tireWorldvel = m_RB.GetPointVelocity(m_Wheel.position);

				//Suspension rest distance?
                Vector3 springVec = transform.position - transform.parent.position;

				//Something is wrong with this >.<
				Debug.DrawRay(transform.parent.position, transform.position);

				float suspensionRestDist = springVec.magnitude;

				float offset = suspensionRestDist - hit.distance;

				float vel = Vector3.Dot(springDir, tireWorldvel);

				float force = (offset * springStrength) - (vel * springDamper);
				m_RB.AddForceAtPosition(springDir * force, m_Wheel.position);

				if(hit.distance > m_SpringSize)
				{
					hit.distance = m_SpringSize;
				}
				if(hit.distance <= m_SpringSize)
				{					
                    Vector3 newLocalPos = transform.InverseTransformPoint(hit.point) + new Vector3(0, m_SpringSize, 0);
					m_Wheel.localPosition = newLocalPos;                  
                }
			}
		}
	}

	private void OnDrawGizmos()
	{
		//Debug.DrawRay(transform.position, -transform.up, Color.red, m_SpringSize);
		//Debug.DrawLine(transform.position, transform.up, Color.red, m_SpringSize);
	}
}

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

	[SerializeField] float stiffness;
	[SerializeField] float damping;

	private SuspensionSO m_Data;
	private float m_SpringSize = 1.5f;
	private bool m_Grounded;

	public void Init(SuspensionSO inData)
	{
		m_Data = inData;
		stiffness = m_Data.SuspensionStrength;
		damping = m_Data.SuspensionDamper;
	}

	public bool GetGrounded()
	{
		Debug.DrawRay(transform.position, -transform.up, Color.green, m_SpringSize);
		RaycastHit hit;

        bool grounded = Physics.Raycast(transform.position, -transform.up, out hit, m_SpringSize, m_LayerMask);
		if (grounded)
			//Debug.Log("Tank is grounded");

        if (grounded != m_Grounded)
        {
            m_Grounded = grounded;
            OnGroundedChanged?.Invoke(m_Grounded);
        }

        return m_Grounded;
    }

	private void FixedUpdate()
	{
		Vector3 direction = Vector3.down;
		Vector3 localDir = transform.TransformDirection(direction);
		bool grounded = GetGrounded();

		Vector3 springVec = transform.position - transform.parent.position;
		float susOffset = m_SpringSize - Vector3.Dot(springVec, localDir);

		m_Wheel.localPosition = Vector3.up * (m_SpringSize - susOffset);

		if (grounded)
		{
			Vector3 worldVec = m_RB.GetPointVelocity(transform.position);
			float susVel = Vector3.Dot(localDir, worldVec);
			float susForce = (susOffset * stiffness) - (susVel * damping);
			m_RB.AddForce(localDir * (susForce / m_RB.mass));

		}


		//float susForce = (susOffset * m_Data.SuspensionStrength) - (susVel * m_Data.SuspensionDamper);

	}
}

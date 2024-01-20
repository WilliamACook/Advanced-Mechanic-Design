using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform m_SpringArmTarget;
	[SerializeField] private Transform m_CameraMount;
	[SerializeField] private Camera m_Camera;
	[SerializeField] private LayerMask m_LayerMask;

	private float m_CameraDist = 5f;
	private float m_Yaw;
	private float m_Pitch;

	[SerializeField] private float m_YawSensitivity;
	[SerializeField] private float m_PitchSensitivity;
	[SerializeField] private float m_ZoomSensitivity;

	[SerializeField] private float m_MaxDist;
	[SerializeField] private float m_MinDist;

	[SerializeField] private float m_CameraProbeSize;
	[SerializeField] private Vector3 m_TargetOffset;

	public void RotateSpringArm(Vector2 change)
	{
		m_Yaw += change.x * m_YawSensitivity;
		m_Pitch -= change.y * m_PitchSensitivity;

		// FOV
		//m_Yaw = Mathf.Clamp(m_Yaw, -80f, 80f);
		m_Pitch = Mathf.Clamp(m_Pitch, -80f, 80f);

		m_SpringArmTarget.rotation = Quaternion.Euler(m_Pitch, m_Yaw, 0f);
	}

	public void ChangeCameraDistance(float amount)
	{
		m_CameraDist -= amount * m_ZoomSensitivity;
		m_CameraDist = Mathf.Clamp(m_CameraDist, m_MinDist, m_MaxDist);
	}

	private void LateUpdate()
	{
		Vector3 offset = m_TargetOffset + m_SpringArmTarget.forward * -m_CameraDist; 
		m_CameraMount.position = m_SpringArmTarget.position + offset;

		m_CameraMount.LookAt(m_SpringArmTarget.position);

		RaycastHit hit;
		if (Physics.SphereCast(m_SpringArmTarget.position, m_CameraProbeSize, -m_SpringArmTarget.forward, out hit, m_CameraDist, ~m_LayerMask))
		{
			m_CameraMount.position = hit.point + m_SpringArmTarget.forward * m_CameraProbeSize;
		}

	}
}
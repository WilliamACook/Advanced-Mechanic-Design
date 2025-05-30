 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UIElements;

public class Turret : MonoBehaviour
{
	[SerializeField] private Camera m_Camera;
	[SerializeField] private Transform m_Turret;
	[SerializeField] private Transform m_Barrel;
	[SerializeField] private float m_RotationSpeed;
    [SerializeField] private float m_VerticalRotationSpeed;
	[SerializeField] private float m_MinVerticalAngle = -30f;
	[SerializeField] private float m_MaxVerticalAngle = 30f;

    private TankSO m_Data;
	private bool m_RotationDirty;
	private Coroutine m_CRAimingTurret;

	private void Awake()
	{
	}

	public void Init(TankSO inData)
	{
		m_Data = inData;
	}

    private void Start()
    {
    }

    public void SetRotationDirty()
	{
		m_RotationDirty = true;
	}

    private void Update()
    {
        //Vector3 camFwdVec = m_Camera.transform.forward;

        //Vector3 projectedVec = Vector3.ProjectOnPlane(camFwdVec, transform.parent.up);

        //Quaternion targetRot = Quaternion.LookRotation(projectedVec, transform.parent.up);

        //Debug.DrawLine(transform.position, transform.position + projectedVec * 50, Color.blue);

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, m_rotationSpeed * Time.deltaTime);
		if(m_RotationDirty)
		{
			if(m_CRAimingTurret != null)
			{
				StopCoroutine(m_CRAimingTurret);
			}

			m_CRAimingTurret = StartCoroutine(C_AimTurret());
		}
    }

    private IEnumerator C_AimTurret()
	{
        Vector3 camFwdVec = m_Camera.transform.forward;
		Vector3 camUpVec = m_Camera.transform.forward;

		camFwdVec.y = 0f;
		camUpVec.z = 0f;

        Quaternion targetRot = Quaternion.LookRotation(camFwdVec);
		Quaternion barrelTargetRot = Quaternion.LookRotation(camUpVec);

		m_Turret.rotation = Quaternion.Slerp(m_Turret.rotation, targetRot, m_RotationSpeed * Time.deltaTime);

		// Barrel Roatation
		// Use the turrets transform and not camera

		Vector3 camUp = m_Turret.forward;

        float pitch = -m_Camera.transform.localEulerAngles.x;

        // Calculate the target rotation for the barrel
        Quaternion targetBarrelRotation = Quaternion.Euler(pitch, 0, 0);

        // Apply rotation to the barrel only around its local X-axis
        m_Barrel.localRotation = Quaternion.Lerp(m_Barrel.localRotation,targetBarrelRotation, 2 * Time.deltaTime);

        //m_Barrel.rotation = Quaternion.Slerp(m_Barrel.rotation, target, m_MaxVerticalAngle * Time.deltaTime);
        //Vector3 newEulerAngles = m_Barrel.rotation.eulerAngles;
        //newEulerAngles.x = 0;
        //newEulerAngles.z = 0;
        //m_Barrel.rotation = Quaternion.Euler(newEulerAngles);
        yield return null;
	}
}

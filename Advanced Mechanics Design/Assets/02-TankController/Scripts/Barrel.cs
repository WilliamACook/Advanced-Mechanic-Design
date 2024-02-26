using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	[SerializeField] private TankSO m_Data;
	[SerializeField] private Shell m_ShellPrefab;
	[SerializeField] private ShellSO[] m_AmmoTypes;
	[SerializeField] private int[] m_AmmoCounts;
	private int m_SelectedShell;

	private float m_CurrentDispersion;

	public void Init(TankSO inData)
	{
		m_Data = inData;
	}

	public void Fire()
	{
		print(transform.localPosition);
		print("Fire");
		var m_Shell = Instantiate(m_ShellPrefab, transform.position, transform.rotation);
		m_Shell.GetComponent<Rigidbody>().velocity = transform.localPosition * m_Data.ShellData.Velocity;

	}

}

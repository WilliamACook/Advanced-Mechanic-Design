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
	private bool m_CanFire = true;

	public void Init(TankSO inData)
	{
		m_Data = inData;
	}

	public void Fire()
	{
		if (!m_CanFire)
			return;

		StartCoroutine(FireCoroutine());
	}

	private IEnumerator FireCoroutine()
	{
		m_CanFire = false;
		var m_Shell = Instantiate(m_ShellPrefab, transform.position, transform.rotation);
		m_Shell.GetComponent<Rigidbody>().velocity = transform.forward * m_Data.ShellData.Velocity;
		yield return new WaitForSeconds(m_Data.BarrelData.ReloadTime);
		m_CanFire = true;
	}

}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Splines;

public class SplineRoad : MonoBehaviour
{
    [SerializeField]
    private int resolution;
	[SerializeField]
	private float m_width;

    [SerializeField] private GameObject cubePrefab;

	[SerializeField]
	int m_StreetLightSpacing = 1;

    private SplineSampler m_splineSampler;
    private MeshFilter m_meshFilter;

    private List<Vector3> m_vertsP1;
    private List<Vector3> m_vertsP2;

    private void Start()
    {
        m_splineSampler = GetComponent<SplineSampler>();
        m_meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        GetVerts();
        BuildMesh();
    }

    private void GetVerts()
    {
        m_vertsP1 = new List<Vector3>();
        m_vertsP2 = new List<Vector3>();

        float step = 1f / (float)resolution;
		Vector3 p1;
		Vector3 p2;

		for(int j = 0; j < m_splineSampler.NumSplines; j++)
		{
			for (int i = 0; i < resolution; i++)
			{
				float t = step * i;
				m_splineSampler.SampleSplineWidth(j, t, m_width, out p1, out p2);
				m_vertsP1.Add(p1);
				m_vertsP2.Add(p2);
			}

			m_splineSampler.SampleSplineWidth(j, 1f, m_width, out p1, out p2);
			m_vertsP1.Add(p1);
			m_vertsP2.Add(p2);
		}


		//for (int i = 0; i < resolution; i++)
		//{
		//    float t = step * i;
		//    m_splineSampler.SampleSplineWidth(t, out Vector3 p1, out Vector3 p2);
		//    m_vertsP1.Add(p1);
		//    m_vertsP2.Add(p2);
		//}
	}

    private void BuildMesh()
    {
        Mesh m = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        int offset = 0;

        int length = m_vertsP2.Count;
        float uvOffset = 0f;

		//for (int i = 1; i <= length; i++)
		//{
		//	Vector3 p1 = m_vertsP1[i - 1];
		//	Vector3 p2 = m_vertsP2[i - 1];
		//	Vector3 p3;
		//	Vector3 p4;

		//	if (i == length)
		//	{
		//		p3 = m_vertsP1[0];
		//		p4 = m_vertsP2[0];
		//	}
		//	else
		//	{
		//		p3 = m_vertsP1[i];
		//		p4 = m_vertsP2[i];
		//	}

		//	offset = 4 * (i - 1);

		//	int t1 = offset + 0;
		//	int t2 = offset + 2;
		//	int t3 = offset + 3;

		//	int t4 = offset + 3;
		//	int t5 = offset + 1;
		//	int t6 = offset + 0;

		//	verts.AddRange(new List<Vector3> { p1, p2, p3, p4 });
		//	tris.AddRange(new List<int> { t1, t2, t3, t4, t5, t6 });

		//	float distance = Vector3.Distance(p1, p2) / 4f;
		//	float uvDistance = uvOffset + distance;
		//	uvs.AddRange(new List<Vector2> { new Vector2(uvOffset, 0), new Vector2(uvOffset, 1), new Vector2(uvDistance, 0), new Vector2(uvDistance, 1) });

		//	uvOffset += distance;
		//}

		for (int currentSplineIndex = 0; currentSplineIndex < m_splineSampler.NumSplines; currentSplineIndex++)
		{
			int splineOffset = resolution * currentSplineIndex;
			splineOffset += currentSplineIndex;
			for (int currentSplinePoint = 1; currentSplinePoint < resolution + 1; currentSplinePoint++)
			{
				int vertoffset = splineOffset + currentSplinePoint;
				Vector3 p1 = m_vertsP1[vertoffset - 1];
				Vector3 p2 = m_vertsP2[vertoffset - 1];
				Vector3 p3 = m_vertsP1[vertoffset];
				Vector3 p4 = m_vertsP2[vertoffset];

				offset = 4 * resolution * currentSplineIndex;
				offset += 4 * (currentSplinePoint - 1);

				int t1 = offset + 0;
				int t2 = offset + 2;
				int t3 = offset + 3;

				int t4 = offset + 3;
				int t5 = offset + 1;
				int t6 = offset + 0;

				verts.AddRange(new List<Vector3> { p1, p2, p3, p4 });
				tris.AddRange(new List<int> { t1, t2, t3, t4, t5, t6 });
			}
		}

		m.SetVertices(verts);
        m.SetTriangles(tris, 0);
        m.RecalculateNormals();

        m.SetUVs(0, uvs);
        m_meshFilter.mesh = m;
    }

    private void OnDrawGizmos()
    {
        if (m_vertsP1 != null && m_vertsP2 != null)
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < m_vertsP1.Count; i++)
            {
                //Gizmos.DrawSphere(m_vertsP1[i], 1f);
				BuildLights(m_vertsP1);
				BuildLights(m_vertsP2);
                Handles.SphereHandleCap(0, m_vertsP1[i] , quaternion.identity, 1f, EventType.Repaint);
                Handles.SphereHandleCap(0, m_vertsP2[i], quaternion.identity, 1f, EventType.Repaint);
                //Gizmos.DrawSphere(m_vertsP2[i], 1f);

                Gizmos.DrawLine(m_vertsP1[i], m_vertsP2[i]);
            }
        }
    }

	private List<GameObject> m_StreetLights = new List<GameObject>();
	private void BuildLights(List<Vector3> points)
	{
		if(points != null)
		{
			foreach(GameObject light in m_StreetLights)
			{
				Destroy(light);
			}
			m_StreetLights.Clear();

			for (int i = 0;i < points.Count; i += m_StreetLightSpacing)
			{
				if(cubePrefab != null)
				{
					Vector3 leftDirection = m_vertsP2[i] - m_vertsP1[i];
					Vector3 rightDirection = m_vertsP1[i] - m_vertsP2[i];

					GameObject leftLights = Instantiate(cubePrefab, m_vertsP1[i], quaternion.identity);
					GameObject rightLights = Instantiate(cubePrefab, m_vertsP2[i], quaternion.identity);

					leftLights.transform.rotation = Quaternion.LookRotation(leftDirection, Vector3.up);
					rightLights.transform.rotation = Quaternion.LookRotation(rightDirection, Vector3.up);

					// Make a List of Streetlights and Destroy them when changed
					m_StreetLights.Add(leftLights);
					m_StreetLights.Add(rightLights);	

					//TODO Make Spacing for streetlights
					
				}
			}
		}
	}
	
}

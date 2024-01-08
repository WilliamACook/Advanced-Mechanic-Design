using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProcedualeMesh : MonoBehaviour
{
	private MeshFilter m_filter;
	private Mesh m_Mesh;
	private List<Vector3> m_vertices;
	private List<int> m_tris;

	private void Awake()
	{
		m_filter = GetComponent<MeshFilter>();
		m_Mesh = new Mesh { name = "Proc Mesh" };
		m_filter.mesh = m_Mesh;

	}

	private void Start()
	{
		m_vertices = new List<Vector3>();
		m_tris = new List<int>();

		m_vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
		m_vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
		m_vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
		m_vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));

		m_vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));
		m_vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
		m_vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
		m_vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));

		//m_vertices.Add(new Vector3(-0.5f, 0.5f, 0));
		//m_vertices.Add(new Vector3(0.5f, 0.5f, 0));
		//m_vertices.Add(new Vector3(0.5f, -0.5f, 0));
		//m_vertices.Add(new Vector3(-0.5f, -0.5f, 0));

		//m_vertices.Add(new Vector3(-0.5f, 0.5f, 0));
		//m_vertices.Add(new Vector3(0.5f, 0.5f, 0));
		//m_vertices.Add(new Vector3(0.5f, -0.5f, 0));
		//m_vertices.Add(new Vector3(-0.5f, -0.5f, 0));

		//m_vertices.Add(new Vector3(-0.5f, 0.5f, 0));
		//m_vertices.Add(new Vector3(0.5f, 0.5f, 0));
		//m_vertices.Add(new Vector3(0.5f, -0.5f, 0));
		//m_vertices.Add(new Vector3(-0.5f, -0.5f, 0));

		//m_vertices.Add(new Vector3(-0.5f, 0.5f, 0));
		//m_vertices.Add(new Vector3(0.5f, 0.5f, 0));
		//m_vertices.Add(new Vector3(0.5f, -0.5f, 0));
		//m_vertices.Add(new Vector3(-0.5f, -0.5f, 0));

		m_tris.AddRange(new List<int>() { 0, 1, 3 , 1, 2, 3}); //Front -z

		m_tris.AddRange(new List<int>() { 0, 4, 7 , 7, 3, 0}); //left side

		m_tris.AddRange(new List<int>() { 1, 5, 6 , 5, 6, 2}); // right side

		m_tris.AddRange(new List<int>() { 4, 5, 6 , 6, 7, 4}); // Back

		m_tris.AddRange(new List<int>() { 0, 4, 5 , 5, 1, 0}); //top

		m_tris.AddRange(new List<int>() { 3, 7, 2 , 7, 6, 2}); //bottom

		//m_tris.Add(0);
		//m_tris.Add(4);
		//m_tris.Add(5);

		//m_tris.Add(5);
		//m_tris.Add(2);
		//m_tris.Add(0);

		//m_tris.Add(5);
		//m_tris.Add(2);
		//m_tris.Add(1);

		m_Mesh.Clear();
		m_Mesh.SetVertices(m_vertices);
		m_Mesh.SetTriangles(m_tris, 0);
		m_Mesh.RecalculateNormals();


	}

}
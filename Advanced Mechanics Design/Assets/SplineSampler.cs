using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode()]
public class SplineSampler : MonoBehaviour
{
    [SerializeField]
    private SplineContainer m_splineContainer;
    [SerializeField]
    private int m_splineIndex;
    [SerializeField]
    public int NumSplines;
    [SerializeField]
    [Range(0f, 1f)]
    private float m_time;
    
    private float m_width = 1f;

    float3 position;
    float3 tangent;
    float3 upVector;

    float3 forward;

    float3 p1;
    float3 p2;

    private void Update()
    {
        if (m_splineContainer != null)
        {
            m_splineContainer.Evaluate(m_splineIndex, m_time, out position, out forward, out upVector);
            float3 right = Vector3.Cross(forward, upVector).normalized;
            p1 = position + (right * m_width);
            p2 = position + (-right * m_width);
			NumSplines = m_splineContainer.Splines.Count;
        }

    }



    private void OnDrawGizmos()
    {
        Handles.SphereHandleCap(0, p1, quaternion.identity, 1f, EventType.Repaint);
        Handles.SphereHandleCap(0, p2, quaternion.identity, 1f, EventType.Repaint);

        Gizmos.DrawLine(p1, p2);
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(p1, 1f);

        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(p2, 1f);
    }

    public void SampleSplineWidth(int SplineIndex, float SampleTValue, float width, out Vector3 sampledP1, out Vector3 sampledP2)
    {
        if (m_splineContainer != null)
        {
			//Debug.Log(m_splineContainer.Splines.Count);
            m_splineContainer.Evaluate(SplineIndex, SampleTValue, out position, out forward, out upVector);

            float3 right = Vector3.Cross(forward, upVector).normalized;
            sampledP1 = position + (right * width);
            sampledP2 = position + (-right * width);
        }
        else
        {
            sampledP1 = float3.zero;
            sampledP2 = float3.zero;
        }


    }
}

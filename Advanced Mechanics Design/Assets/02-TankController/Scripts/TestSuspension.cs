using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSuspension : MonoBehaviour
{
	private Rigidbody rb;
	[SerializeField] float  springLength;
	[SerializeField] float stiffness;
	[SerializeField] float damping;

	// Start is called before the first frame update
	void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 direction = Vector3.down;

		Vector3 localDir = transform.TransformDirection(direction);

		Vector3 worldVec = rb.GetPointVelocity(transform.position);
			
		Vector3 springVec = transform.position - transform.parent.position;

		float susOffset = springLength - Vector3.Dot(springVec, localDir);

		float susVel = Vector3.Dot(localDir, worldVec);

		float susForce = (susOffset * stiffness) - (susVel * damping);

		rb.AddForce(localDir * (susForce / rb.mass));
		//Debug.Log(localDir * (susForce / rb.mass));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CelestialBody : MonoBehaviour
{
	public Vector3 initialVelocity;

	public float raiuds;

	const float G = 667.4f;

	public static List<CelestialBody> CelestialBodies;

	public Rigidbody rb;

    private void Start()
    {
		rb.velocity = initialVelocity;
    }

    void FixedUpdate()
	{
		if (Application.isPlaying)
		{

			foreach (CelestialBody attractor in CelestialBodies)
			{
				if (attractor != this)
					Attract(attractor);
			}
		}
	}

    void Update()
    {
		if (!Application.isPlaying)
		{
			transform.localScale = new Vector3(raiuds, raiuds, raiuds);
		}
	}

    void OnEnable()
	{
		if (CelestialBodies == null)
			CelestialBodies = new List<CelestialBody>();

		CelestialBodies.Add(this);
	}

	void OnDisable()
	{
		CelestialBodies.Remove(this);
	}

	void Attract(CelestialBody objToAttract)
	{
		Rigidbody rbToAttract = objToAttract.rb;

		float disctance = Vector3.Distance(transform.position, objToAttract.transform.position);

		Vector3 direction = rb.position - rbToAttract.position;
		float distance = direction.magnitude;

		if (distance == 0f)
			return;

		float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
		Vector3 force = direction.normalized * forceMagnitude;

		rbToAttract.AddForce(force);
	}

}
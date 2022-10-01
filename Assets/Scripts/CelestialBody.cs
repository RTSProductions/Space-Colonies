using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CelestialBody : MonoBehaviour
{
	public Vector3 initialVelocity;

	public float raiuds;

	public float surfaceGravity = 10;

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
			foreach (GravityObject rb in GravityObject.gravityObjects)
			{
				AttractOBJ(rb.rb);
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

		Vector3 direction = rb.position - rbToAttract.position;
		float distance = direction.sqrMagnitude;

		if (distance == 0f)
			return;

		float forceMagnitude = G * (rb.mass * rbToAttract.mass) / distance; // Mathf.Pow(distance, 2);
		Vector3 force = direction.normalized * forceMagnitude;

		rbToAttract.AddForce(force);
	}

	void AttractOBJ(Rigidbody objToAttract)
	{
		Rigidbody rbToAttract = objToAttract.GetComponent<Rigidbody>();

		float distance = Vector3.Distance(transform.position, objToAttract.transform.position);

		if (distance > raiuds * 5f)
            return;

		Vector3 targetDir = (objToAttract.transform.position - transform.position).normalized;
		Vector3 bodyUp = objToAttract.transform.up;
		if (rbToAttract.transform.tag == "Player" || rbToAttract.transform.tag == "Creature")
		{
			if (Quaternion.Angle(rbToAttract.transform.rotation, Quaternion.FromToRotation(bodyUp, targetDir) * objToAttract.transform.rotation) >= 50)
				objToAttract.transform.rotation = Quaternion.Slerp(rbToAttract.transform.rotation, Quaternion.FromToRotation(bodyUp, targetDir) * objToAttract.transform.rotation, 1 * Time.deltaTime);
			else
				objToAttract.transform.rotation = Quaternion.FromToRotation(bodyUp, targetDir) * objToAttract.transform.rotation;

		}
		rbToAttract.AddForce(targetDir * -surfaceGravity);

	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	public Vector3 normal;
	public Vector3 incomingVec;
	public float ior;
	public float kt;

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine (Vector3.zero, normal.normalized);
		Gizmos.color = Color.red;
		Gizmos.DrawLine (incomingVec.normalized, Vector3.zero);
		Vector3 n = normal;
		float cos_theta = Vector3.Dot (n.normalized, incomingVec.normalized);
		float eta = ior;
		float tir = 1.0f - (1.0f - cos_theta * cos_theta) / (eta * eta);
		float cos_theta2 = Mathf.Sqrt (tir);
		Vector3 wt = -incomingVec / eta - (cos_theta2 - cos_theta / eta) * n;
		Gizmos.color = Color.green;
		Debug.Log (wt.normalized);
		Gizmos.DrawLine(Vector3.zero,wt.normalized);
	}
}

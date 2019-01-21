using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FresnelTransmitter : BTDF
{
	public float ior = 0.0f;
	public float kt = 0.0f;

	public FresnelTransmitter()
	{
	}

	public void set_kt(float kt)
	{
		this.kt = kt;
	}

	public void set_ior(float ior)
	{
		this.ior = ior;
	}

	public override bool tir(ref Shade sr)
	{
		Vector3 wo = -sr.ray.direction;
		float cos_theta = Vector3.Dot (sr.normal, wo);
		float eta = ior;
		if (cos_theta < 0)
			eta = 1.0f / eta;
		if((1.0f -  (1.0f - cos_theta * cos_theta)/(eta * eta) < 0))
			return true;
		return false;
	}

	public override Color sample_f(ref Shade sr, ref Vector3 wo,ref Vector3 wt,float kr)
	{
		Vector3 n = sr.normal;
		float cos_theta = Vector3.Dot (n, wo);
		float eta = ior;
		if (cos_theta < 0) {
			cos_theta = -cos_theta;
			n = -n;
			eta = 1.0f / eta;
		}
		float temp = 1.0f - (1.0f - cos_theta * cos_theta) / (eta * eta);
		float cos_theta2 = Mathf.Sqrt (temp);
		wt = -wo / eta - (cos_theta2 - cos_theta / eta) * n;
		kt = 1 - kr;
		return (kt / (eta * eta) * Constants.white / Mathf.Abs (Vector3.Dot(sr.normal, wt)));
	}
}
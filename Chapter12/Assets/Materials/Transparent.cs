using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparent : Phong 
{
	public PerfectTransmitter 	specular_btdf = null;
	public PerfectSpecular		reflective_brdf = null;

	public Transparent()
	{
		specular_btdf = new PerfectTransmitter ();
		reflective_brdf = new PerfectSpecular();
	}

	public void set_kt(float kt)
	{
		specular_btdf.kt = kt;
	}

	public void set_ior(float ior)
	{
		specular_btdf.set_ior (ior);
	}

	public void set_kr(float k)
	{
		reflective_brdf.set_kr(k);
	}

	public	void set_cr(Color c)
	{
		reflective_brdf.set_cr (c);
	}


	public override Color shade(ref Shade sr)
	{
		Color L = Constants.black;  

		Vector3 wo = -sr.ray.direction;
		Vector3 wi = Vector3.zero;
		Color fr = reflective_brdf.sample_f (ref sr, ref wo, ref wi);
		Ray reflected_ray = new Ray (sr.hit_point, wi);
		if(specular_btdf.tir(ref sr))
			L += sr.w.tracer_ptr.trace_ray(reflected_ray,sr.depth+1);
		else
		{
			Vector3 wt = Vector3.zero;
			Color ft = specular_btdf.sample_f (ref sr, ref wo, ref wt);
			Ray transmitted_ray = new Ray (sr.hit_point, wt.normalized);
			L += (fr * sr.w.tracer_ptr.trace_ray (reflected_ray, sr.depth + 1) * Mathf.Abs(Vector3.Dot(sr.normal,wi)));
			L += (ft * sr.w.tracer_ptr.trace_ray (transmitted_ray,sr.depth + 1) * Mathf.Abs(Vector3.Dot(sr.normal,wt)));
		}
		return L;
	}
}
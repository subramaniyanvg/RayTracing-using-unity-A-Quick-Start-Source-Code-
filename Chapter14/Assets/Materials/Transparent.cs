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

	public void set_ks(float k)
	{
		specular_brdf.set_ks(k);
	}

	public void set_exp(float e)
	{
		specular_brdf.set_exp(e);
	}

	public void set_cs(Color c)
	{
		specular_brdf.set_cs (c);
	}

	public void set_ka(float k)
	{
		ambient_brdf.set_kd (k);
	}

	public void set_kd(float k)
	{
		diffuse_brdf.set_kd (k);
	}

	public void	set_cd(Color c)
	{
		ambient_brdf.set_cd (c);
		diffuse_brdf.set_cd (c);
	}

	public override Color shade(ref Shade sr)
	{
		Phong phongMat = new Phong ();
		Color L = phongMat.shade(ref sr);
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
			Ray transmitted_ray = new Ray (sr.hit_point, wt);
			L += fr * sr.w.tracer_ptr.trace_ray (reflected_ray, sr.depth + 1) * Mathf.Abs(Vector3.Dot(sr.normal,wi));
			L += ft * sr.w.tracer_ptr.trace_ray (transmitted_ray, sr.depth + 1) * Mathf.Abs(Vector3.Dot(sr.normal,wt));
		}
		return L;
	}
}
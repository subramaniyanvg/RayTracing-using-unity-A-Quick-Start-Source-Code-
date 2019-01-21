using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Reflective: Phong 
{	
	public Reflective()
	{
		reflective_brdf = new PerfectSpecular ();
	}

	public void set_kr(float k)
	{
		reflective_brdf.set_kr(k);
	}

	public void	set_cr(Color c)
	{
		reflective_brdf.set_cr(c);
	}

	public override Color shade(ref Shade s)
	{
		Phong phongMat = new Phong ();
		phongMat.set_ka (0.5f);
		phongMat.set_kd(1.0f);	
		phongMat.set_ks(0.8f);
		phongMat.set_exp(10.0f);	
		phongMat.set_cd(new Color(1,1,0,1));
		phongMat.set_cs (Constants.white);
		Color L = phongMat.shade(ref s);  //Get color value from Phong shade

		Vector3 wo = -s.ray.direction;
		Vector3 wi = Vector3.zero;	
		Color fr = reflective_brdf.sample_f(ref s,ref wo,ref wi); 
		Ray reflected_ray = new Ray(s.hit_point, wi); 
		L += fr * s.w.tracer_ptr.trace_ray(reflected_ray, s.depth + 1) * (Vector3.Dot(s.normal,wi));
		return (L);
	}

	public override Color path_shade(ref Shade s)
	{
		Vector3 wo = -s.ray.direction;
		Vector3 wi = Vector3.zero;
		float pdf = 0.0f;
		Color fr = reflective_brdf.sample_f(ref s,ref wo,ref wi,ref pdf);
		Ray reflected_ray = new Ray(s.hit_point,wi);
		Color L = fr * s.w.tracer_ptr.trace_ray (reflected_ray, s.depth + 1) * Vector3.Dot (s.normal, wi) / pdf;
		return L;
	}

	public override Color global_shade(ref Shade sr)
	{
		Vector3 wo = -sr.ray.direction;
		Vector3 wi = Vector3.zero;
		float pdf = 0.0f;
		Color fr = reflective_brdf.sample_f (ref sr, ref wo, ref wi, ref pdf);
		Ray reflected_ray = new Ray (sr.hit_point, wi);
		if (sr.depth == 0)
			return fr * sr.w.tracer_ptr.trace_ray (reflected_ray, sr.depth + 2) * Vector3.Dot (sr.normal, wi) / pdf;
		return fr * sr.w.tracer_ptr.trace_ray(reflected_ray,sr.depth + 1) * Vector3.Dot (sr.normal, wi) / pdf;
	}
	PerfectSpecular reflective_brdf;		
}
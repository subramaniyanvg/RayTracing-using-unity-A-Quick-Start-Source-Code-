using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SV_Reflective : Phong 
{	
	public SV_Reflective()
	{
		reflective_brdf = new SV_PerfectSpecular ();
	}

	public void set_kr(float k)
	{
		reflective_brdf.set_kr(k);
	}

	public void	set_cr(TextureData c)
	{
		reflective_brdf.set_cr(c);
	}

	public override Color shade(ref Shade s)
	{
		Color L = Constants.black;  //Get color value from Phong shade

		Vector3 wo = -s.ray.direction;
		Vector3 wi = Vector3.zero;	
		Color fr = reflective_brdf.sample_f(ref s,ref wo,ref wi); 
		Ray reflected_ray = new Ray(s.hit_point, wi); 
		L += fr * s.w.tracer_ptr.trace_ray(reflected_ray, s.depth + 1,fr) * (Vector3.Dot(s.normal,wi));
		return (L);
	}

	public override Color path_shade(ref Shade s)
	{
		Vector3 wo = -s.ray.direction;
		Vector3 wi = Vector3.zero;
		float pdf = 0.0f;
		Color fr = reflective_brdf.sample_f(ref s,ref wo,ref wi,ref pdf);
		Ray reflected_ray = new Ray(s.hit_point,wi);
		return fr * s.w.tracer_ptr.trace_ray (reflected_ray, s.depth + 1) * Vector3.Dot (s.normal, wi) / pdf;
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
	SV_PerfectSpecular reflective_brdf;		
}
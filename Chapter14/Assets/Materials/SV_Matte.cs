using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SV_Matte : Matrial 
{
	public SV_Lambertian		ambient_brdf = null;
	public SV_Lambertian		diffuse_brdf = null;

	public SV_Matte():base()
	{
		ambient_brdf = new SV_Lambertian();
		diffuse_brdf = new SV_Lambertian();
	}

	public void set_ka(float k)
	{
		ambient_brdf.set_kd (k);
	}

	public void set_kd(float k)
	{
		diffuse_brdf.set_kd (k);
	}

	public void	set_cd(TextureData c)
	{
		ambient_brdf.set_cd (c);
		diffuse_brdf.set_cd (c);
	}

	public override Color shade(ref Shade sr)
	{
		Vector3 wo 	= -sr.ray.direction;
		Color L = ambient_brdf.rho(ref sr,ref wo) * sr.w.ambient_ptr.L(ref sr);
		int 	num_lights	= sr.w.lights.Count;

		for (int j = 0; j < num_lights; j++) 
		{
			Vector3 wi = sr.w.lights[j].get_direction(ref sr);    
			float ndotwi = Vector3.Dot(sr.normal,wi);
			if (ndotwi > 0.0) 
			{
				bool inshadow = false;
				if (sr.w.lights [j].cast_shadows) 
				{
					Ray shadowRay = new Ray (sr.hit_point, wi);
					inshadow =  sr.w.lights[j].in_shadow(ref shadowRay,ref sr);
				}
				if(!inshadow)
					L += diffuse_brdf.f (ref sr, ref wo, ref wi) * sr.w.lights [j].L (ref sr) * ndotwi;
			}
		}

		return (L);
	}

	public override Color area_light_shade(ref Shade sr)
	{
		Vector3 wo 	= -sr.ray.direction;
		Color L =  ambient_brdf.rho(ref sr,ref wo) * sr.w.ambient_ptr.L(ref sr);
		int 	num_lights	= sr.w.lights.Count;

		for (int j = 0; j < num_lights; j++) 
		{
			Vector3 wi = sr.w.lights[j].get_direction(ref sr);    
			float ndotwi = Vector3.Dot(sr.normal,wi);
			if (ndotwi > 0.0) 
			{
				bool inshadow = false;
				if (sr.w.lights [j].cast_shadows) 
				{
					Ray shadowRay = new Ray (sr.hit_point, wi);
					inshadow =  sr.w.lights[j].in_shadow(ref shadowRay,ref sr);
				}
				if(!inshadow)
					L += diffuse_brdf.f(ref sr,ref wo,ref wi) * sr.w.lights[j].L(ref sr) *  sr.w.lights[j].G(ref sr) * ndotwi / sr.w.lights[j].pdf(ref sr);
			}
		}

		return (L);
	}

	public override Color path_shade(ref Shade sr)
	{
		Vector3 wi 	= Vector3.zero;
		Vector3 wo = -sr.ray.direction;
		float pdf = 0.0f;
		Color f = diffuse_brdf.sample_f (ref sr, ref wo, ref wi, ref pdf);
		float notwi = Vector3.Dot (sr.normal, wi);
		Ray reflected_ray = new Ray (sr.hit_point, wi);
		return f* sr.w.tracer_ptr.trace_ray(reflected_ray,sr.depth + 1) * notwi/pdf;
	}

	public override Color global_shade(ref Shade sr)
	{
		Color L = Constants.black;
		if (sr.depth == 0)
			L = area_light_shade(ref sr);
		
		Vector3 wi = Vector3.zero;
		Vector3 wo = -sr.ray.direction;
		float pdf = 0.0f;
		Color f = diffuse_brdf.sample_f (ref sr, ref wo, ref wi, ref pdf);
		float ndotwi = Vector3.Dot (sr.hit_point, wi);
		Ray reflected_ray = new Ray (sr.hit_point, wi);
		L += f * sr.w.tracer_ptr.trace_ray (reflected_ray, sr.depth + 1) * ndotwi / pdf;
		return L;
	}
}
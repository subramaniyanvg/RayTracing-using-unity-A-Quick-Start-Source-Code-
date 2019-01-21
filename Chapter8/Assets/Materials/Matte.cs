using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matte : Matrial 
{
	public Lambertian		ambient_brdf = null;
	public Lambertian		diffuse_brdf = null;

	public Matte():base()
	{
		ambient_brdf = new Lambertian();
		diffuse_brdf = new Lambertian();
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
		Vector3 wo 	= -sr.ray.direction;
		Color 	L 		=  ambient_brdf.rho(ref sr,ref wo) * sr.w.ambient_ptr.L(ref sr);
		int 	num_lights	= sr.w.lights.Count;

		for (int j = 0; j < num_lights; j++) 
		{
			Vector3 wi = sr.w.lights[j].get_direction(ref sr);    
			float ndotwi = Vector3.Dot(sr.normal,wi);
			if (ndotwi > 0.0) 
				L += diffuse_brdf.f(ref sr,ref wo,ref wi) * sr.w.lights[j].L(ref sr) * ndotwi;
		}

		return (L);
	}

	public override Color area_light_shade(ref Shade sr)
	{
		Vector3 wo 	= -sr.ray.direction;
		Color 	L 		=  ambient_brdf.rho(ref sr,ref wo) * sr.w.ambient_ptr.L(ref sr);
		int 	num_lights	= sr.w.lights.Count;

		for (int j = 0; j < num_lights; j++) 
		{
			Vector3 wi = sr.w.lights[j].get_direction(ref sr);    
			float ndotwi = Vector3.Dot(sr.normal,wi);
			if (ndotwi > 0.0) 
				L += diffuse_brdf.f(ref sr,ref wo,ref wi) * sr.w.lights[j].L(ref sr) *  sr.w.lights[j].G(ref sr) * ndotwi / sr.w.lights[j].pdf(ref sr);
		}

		return (L);
	}
}
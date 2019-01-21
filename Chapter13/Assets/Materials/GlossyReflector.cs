using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlossyReflector : Phong 
{
	public void set_samples(int num_Samples,float exp)
	{
		glossy_specular_brdf.set_samples (num_Samples, exp);
	}

	public void set_kr(float k)
	{
		glossy_specular_brdf.set_ks (k);
	}

	public void set_exponent(float exp)
	{
		glossy_specular_brdf.set_exp(exp);
	}

	public override Color area_light_shade(ref Shade sr)
	{
		Phong phongMat = new Phong ();
		phongMat.set_ka (0.5f);
		phongMat.set_kd(1.0f);	
		phongMat.set_ks(0.8f);
		phongMat.set_exp(10.0f);	
		phongMat.set_cd(new Color(1,1,0,1));
		phongMat.set_cs (Constants.white);
		Color L = phongMat.area_light_shade (ref sr);
		Vector3 wo = -sr.ray.direction;
		Vector3 wi = Vector3.zero;
		float pdf = 0;
		Color fr = glossy_specular_brdf.sample_f (ref sr, ref wo, ref wi, ref pdf);
		Ray reflected_ray = new Ray (sr.hit_point, wi);
		L += fr * sr.w.tracer_ptr.trace_ray (reflected_ray, sr.depth + 1) * Vector3.Dot (sr.normal, wi) / pdf;
		return L;
	}

	GlossySpecular glossy_specular_brdf;
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dielectric : Phong 
{
	public Color 				cf_in;
	public Color 				cf_out;
	public FresnelReflector 	fresnel_brdf = null;
	public FresnelTransmitter	fresnel_btdf = null;

	public void set_cf_in(Color cfin)
	{
		cf_in = cfin;
	}

	public void set_cf_out(Color cfout)
	{
		cf_out = cfout;
	}

	public Dielectric()
	{
		fresnel_brdf = new FresnelReflector ();
		fresnel_btdf = new FresnelTransmitter();
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
		Vector3 wi = Vector3.zero;
		Vector3 wo = -sr.ray.direction;
		Color fr = fresnel_brdf.sample_f (ref sr, ref wo, ref wi);
		Ray reflected_ray = new Ray (sr.hit_point, wi);
		float t = 0.0f;
		Color Lr = Constants.black;
		Color Lt = Constants.black;
		float ndotwi = Vector3.Dot (sr.normal, wi);
		if (fresnel_btdf.tir (ref sr)) 
		{
			if (ndotwi < 0) {
				Lr = sr.w.tracer_ptr.trace_ray (reflected_ray,ref t, sr.depth + 1);
				L += new Color (Mathf.Pow (cf_in.r, t), Mathf.Pow (cf_in.g, t), Mathf.Pow (cf_in.b, t), 1.0f) * Lr;
			} else {
				Lr = sr.w.tracer_ptr.trace_ray (reflected_ray,ref t, sr.depth + 1);
				L += new Color (Mathf.Pow (cf_out.r, t), Mathf.Pow (cf_out.g, t), Mathf.Pow (cf_out.b, t), 1.0f) * Lr;
			}
		} 
		else 
		{
			Vector3 wt = Vector3.zero;
			Color ft = fresnel_btdf.sample_f (ref sr, ref wo, ref wt);
			Ray transmitted_ray = new Ray (sr.hit_point, wt);
			float ndotwt = Vector3.Dot(sr.normal,wt);
			if (ndotwi < 0) {
				Lr = fr * sr.w.tracer_ptr.trace_ray (reflected_ray,ref t, sr.depth + 1) * Mathf.Abs (ndotwi);
				L += new Color (Mathf.Pow (cf_in.r, t), Mathf.Pow (cf_in.g, t), Mathf.Pow (cf_in.b, t), 1.0f) * Lr;

				Lt = ft * sr.w.tracer_ptr.trace_ray (transmitted_ray,ref t, sr.depth + 1) * Mathf.Abs (ndotwt);
				L += new Color (Mathf.Pow (cf_out.r, t), Mathf.Pow (cf_out.g, t), Mathf.Pow (cf_out.b, t), 1.0f) * Lt;
			}
			else 
			{
				Lr = fr * sr.w.tracer_ptr.trace_ray (reflected_ray,ref t, sr.depth + 1) * Mathf.Abs (ndotwi);
				L += new Color (Mathf.Pow (cf_out.r, t), Mathf.Pow (cf_out.g, t), Mathf.Pow (cf_out.b, t), 1.0f) * Lr;

				Lt = fr * sr.w.tracer_ptr.trace_ray (reflected_ray,ref t, sr.depth + 1) * Mathf.Abs (ndotwt);
				L += new Color (Mathf.Pow (cf_in.r, t), Mathf.Pow (cf_in.g, t), Mathf.Pow (cf_in.b, t), 1.0f) * Lt;
			}
		}
		return L;
	}
}
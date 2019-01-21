using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lambertian:BRDF 
{
	public Sampler		sampler_ptr;
	public float		exp;
	public float		kd;
	public  Color 	cd;

	public void Set_Sampler(int numSamples,float exp)
	{
		sampler_ptr = new Jittered (numSamples);
		this.exp = exp;
		sampler_ptr.map_samples_to_hemisphere (exp);
	}

	public override Color f(ref Shade sr,ref Vector3 wo,ref Vector3 wi)
	{
		return (kd * cd * Constants.invPI);
	}

	public override Color sample_f(ref Shade sr, ref Vector3 wo,ref Vector3 wi,ref float pdf)
	{
		Vector3 w = sr.normal;
		Vector3 v = Vector3.Cross (new Vector3 (0.0034f, 1.0f, 0.0071f), w);
		v.Normalize ();
		Vector3 u = Vector3.Cross (v, w);
		Vector3 sp = sampler_ptr.sample_hemisphere ();
		wi = sp.x * u + sp.y * v + sp.z * w;
		wi.Normalize ();
		pdf = Vector3.Dot (sr.normal, wi) * Constants.invPI;
		return (kd * cd * Constants.invPI);
	}

	public override Color rho(ref Shade sr,ref Vector3 wo)
	{
		return (kd * cd);
	}

	public void set_ka(float k)	
	{
		kd = k;
	}

	public void set_kd(float k)
	{
		kd = k;
	}

	public void set_cd(Color c)
	{
		cd = c;
	}
}
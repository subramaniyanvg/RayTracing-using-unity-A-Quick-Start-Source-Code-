using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SV_Lambertian:BRDF 
{
	public Sampler		sampler_ptr;
	public float		exp;
	public float		kd;
	public  TextureData cd;

	public void Set_Sampler(int numSamples,float exp)
	{
		sampler_ptr = new Jittered (numSamples);
		this.exp = exp;
		sampler_ptr.map_samples_to_hemisphere (exp);
	}

	public override Color f(ref Shade sr,ref Vector3 wo,ref Vector3 wi)
	{
		return (kd * cd.get_color(ref sr) * Constants.invPI);
	}

	public override Color rho(ref Shade sr,ref Vector3 wo)
	{
		return (kd * cd.get_color(ref sr));
	}

	public void set_ka(float k)	
	{
		kd = k;
	}

	public void set_kd(float k)
	{
		kd = k;
	}

	public void set_cd(TextureData cd)
	{
		this.cd = cd;
	}
}
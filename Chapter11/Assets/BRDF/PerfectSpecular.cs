using UnityEngine;
using System;

class PerfectSpecular: BRDF 
{
	public	PerfectSpecular()
	{
		kr = 0.0f;
		cr = Constants.white;
	}
		
	public void set_kr(float k)
	{
		kr = k;
	}

		
	public	void set_cr(Color c)
	{
		cr = c;
	}
		
	public override Color f(ref Shade sr, ref Vector3 wo, ref Vector3 wi)
	{
		return Constants.black;
	}
		
	public override Color sample_f(ref Shade sr, ref Vector3 wo,ref Vector3 wi)
	{
		float ndotwo = Vector3.Dot(sr.normal,wo);
		wi = -wo + 2.0f * sr.normal * ndotwo; 
		return (kr * cr / Vector3.Dot(sr.normal,wi)); 
	}
		
	public override Color sample_f(ref Shade sr, ref Vector3 wo,ref Vector3 wi,ref float pdf)
	{
		float ndotwo = Vector3.Dot(sr.normal,wo);
		wi = -wo + 2.0f* sr.normal * ndotwo; 
		pdf = Vector3.Dot(sr.normal,wi);
		return (kr * cr);  
	}
		
	public override Color rho(ref Shade sr,ref Vector3 wo)
	{
		return Constants.black;
	}
				
	float		kr;			// reflection coefficient
	Color 		cr;			// the reflection colour
}
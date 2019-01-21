using UnityEngine;
using System;

public class FresnelReflector: BRDF 
{
	public float eta_out;
	public float eta_in;

	public void set_eta_in(float etain)
	{
		eta_in = etain;
	}

	public void set_eta_out(float etaout)
	{
		eta_out = etaout;
	}

	public float fresnel(ref Shade sr)
	{
		Vector3 normal = sr.normal;
		float ndotd = Vector3.Dot (-normal, sr.ray.direction);
		float eta = 0.0f;
		if (ndotd < 0) 
		{
			normal = -normal;
			eta = eta_out / eta_in;
		}
		else
			eta = eta_in / eta_out;
	
		float cos_theta_i = Vector3.Dot (-normal, sr.ray.direction);
		float temp = 1.0f - (1.0f - cos_theta_i * cos_theta_i) / (eta * eta);
		float cos_theta_t = Mathf.Sqrt(1.0f - (1.0f - cos_theta_i * cos_theta_i)/(eta * eta));
		float r_parallel = (eta * cos_theta_i - cos_theta_t) / (eta * cos_theta_i + cos_theta_t);
		float r_perpendicular = (cos_theta_i - eta * cos_theta_t) / (cos_theta_i + eta * cos_theta_t);
		kr = 0.5f * (r_parallel * r_parallel + r_perpendicular * r_perpendicular);
		return kr;
	}

	public	FresnelReflector()
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
		return (kr * cr / Mathf.Abs(Vector3.Dot(sr.normal,wi))); 
			
	}
		
	public override Color sample_f(ref Shade sr, ref Vector3 wo,ref Vector3 wi,ref float pdf)
	{
		float ndotwo = Vector3.Dot(sr.normal,wo);
		wi = -wo + 2.0f* sr.normal * ndotwo; 
		pdf = Mathf.Abs(Vector3.Dot(sr.normal,wi));
		return (kr * cr);  
	}
		
	public override Color rho(ref Shade sr,ref Vector3 wo)
	{
		return Constants.black;
	}
				
	float		kr;			// reflection coefficient
	Color 		cr;			// the reflection colour
}
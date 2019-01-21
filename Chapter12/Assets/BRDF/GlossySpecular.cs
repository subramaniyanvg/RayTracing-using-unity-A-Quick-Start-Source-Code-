using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlossySpecular : BRDF
{
	Sampler         sampler;
	public float	ks;			//specular scale
	public Color 	cs;			// specular color
	public float	exp; 		// specular exponent

	public void set_samples(int numSamples,float exp)
	{
		sampler = new Jittered(numSamples);
		sampler.map_samples_to_hemisphere (exp);
	}

	public override Color f(ref Shade sr, ref Vector3 wo, ref Vector3 wi)
	{
		Color 	L = Color.black;  				
		float 		ndotwi = Vector3.Dot(sr.normal,wi);
		Vector3 	r = -wi + 2.0f * sr.normal * ndotwi; // mirror reflection direction
		float 		rdotwo = Vector3.Dot(r,wo); 		

		if (rdotwo > 0.0f)
			L = ks * cs * Mathf.Pow(rdotwo, exp); 

		return (L);
	}

	public override Color sample_f(ref Shade sr, ref Vector3 wo,ref Vector3 wi,ref float pdf)
	{
		float ndotwo = Vector3.Dot(sr.normal,wo);
		Vector3 r = -wo + 2.0f * sr.normal * ndotwo;
		Vector3 w = r;
		Vector3 u = Vector3.Cross (new Vector3 (0.00424f, 1.0f, 0.00674f), w);
		u.Normalize ();
		Vector3 v = Vector3.Cross (u, w);
		Vector3 sp = sampler.sample_hemisphere ();
		wi = sp.x * u + sp.y * v + sp.z * w;
		if (Vector3.Dot (sr.normal , wi) < 0)
			wi = -sp.x * u - sp.y * v + sp.z * w;
		float phonglobe = Mathf.Pow (Vector3.Dot (r, wi), exp);
		pdf = phonglobe * (Vector3.Dot (sr.normal, wi));

		return (ks * cs * phonglobe);  
	}


	public override Color rho(ref Shade sr, ref Vector3 wo)
	{
		return Constants.black;
	}

	public void set_ks(float k)
	{
		ks = k;
	}

	public void set_exp(float e)
	{
		exp= e;
	}

	public void set_cs(Color c)
	{
		cs = c;
	}
}
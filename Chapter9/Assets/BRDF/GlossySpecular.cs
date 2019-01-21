using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlossySpecular : BRDF
{
	public float	ks;			//specular scale
	public Color 	cs;			// specular color
	public float	exp; 		// specular exponent

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
		return Constants.black;
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
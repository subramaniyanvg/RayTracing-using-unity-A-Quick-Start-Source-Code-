using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lambertian:BRDF 
{
	public float		kd;
	public  Color 	cd;

	public override Color f(ref Shade sr,ref Vector3 wo,ref Vector3 wi)
	{
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
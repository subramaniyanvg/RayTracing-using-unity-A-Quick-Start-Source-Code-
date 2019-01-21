using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emissive : Matrial
{
	public float 	ls;
	public Color 	ce;

	public Emissive():base()
	{
	}

	public void scale_radiance(float b)
	{
		ls = b;
	}

	public void set_ce(Color c)
	{
		ce  = c; 
	}

	public override Color get_le()
	{
		return (ls * ce);
	}

	public override Color shade(ref Shade sr)
	{
		return Constants.black;
	}

	public override Color area_light_shade(ref Shade sr)
	{
		if (Vector3.Dot (-sr.normal, sr.ray.direction) > 0)
			return (ls * ce);
		else
			return Constants.black;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientOccluder : Lighting 
{
	public Vector3 	u, v, w;
	public Sampler	sampl_ptr;
	public float	ls;			
	public Color	minAmount;
	public Color	color;

	public void scale_radiance(float b)
	{
		ls = b;
	}

	public void set_color(Color c)
	{
		color  = c; 
	}

	public void set_minAmount(Color minAmount)
	{
		this.minAmount  = minAmount; 
	}

	public void SetSampler(Sampler samplePtr)
	{
		if (sampl_ptr != null)
			sampl_ptr = null;
		sampl_ptr = samplePtr;
		sampl_ptr.map_samples_to_hemisphere (1);
	}

	public override Vector3	get_direction(ref Shade s)
	{
		Vector3 sp = sampl_ptr.sample_hemisphere ();
		return sp.x * u + sp.y * v + sp.z * w;
	}

	public override Color L(ref Shade s)
	{
		w = s.normal;
		v = Vector3.Cross (w,new Vector3(0.0072f, 1.0f, 0.0034f));
		v.Normalize ();
		u = Vector3.Cross (v, w);
		Ray shadowray = new Ray(s.hit_point,get_direction(ref s));
		if(in_shadow(ref shadowray,ref s))
			return (minAmount * ls * color);
		return (ls * color);
	}

	public override bool in_shadow(ref Ray r,ref Shade sr)
	{
		float t = Mathf.Infinity;
		int numobjects = sr.w.objects.Count;
		for (int j = 0; j < numobjects; j++)
		{
			if (sr.w.objects [j].shadow_hit (ref r, ref t))
				return true;
		}
		return false;
	}
}
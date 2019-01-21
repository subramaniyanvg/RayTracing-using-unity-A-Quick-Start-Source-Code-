using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : Lighting {

	public float	ls;			
	public Color	color;
	public Vector3  location;

	public void scale_radiance(float b)
	{
		ls = b;
	}

	public void set_location(Vector3 loc)
	{
		location = loc;
	}

	public void set_color(Color c)
	{
		color  = c; 
	}

	public override Vector3	get_direction(ref Shade s)
	{
		return (location - s.hit_point).normalized;
	}

	public override Color L(ref Shade s)
	{
		return (ls * color);
	}

	public override bool in_shadow(ref Ray r,ref Shade sr)
	{
		float t = Mathf.Infinity;
		int numobjects = sr.w.objects.Count;
		float d = Vector3.Distance (location, r.origin);
		for (int j = 0; j < numobjects; j++)
		{
			if (sr.w.objects [j].shadow_hit (ref r, ref t) && t < d)
				return true;
		}
		return false;
	}
}
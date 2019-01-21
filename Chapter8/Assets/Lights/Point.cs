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
}
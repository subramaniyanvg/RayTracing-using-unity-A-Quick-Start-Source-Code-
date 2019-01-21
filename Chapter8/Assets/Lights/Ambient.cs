using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambient : Lighting {

	public float	ls;			
	public Color	color;

	public void scale_radiance(float b)
	{
		ls = b;
	}

	public void set_color(Color c)
	{
		color  = c; 
	}

	public override Vector3	get_direction(ref Shade s)
	{
		return Vector3.zero;
	}

	public override Color L(ref Shade s)
	{
		return (ls * color);
	}
}
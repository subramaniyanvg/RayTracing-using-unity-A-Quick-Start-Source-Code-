using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directional : Lighting
{

		public float	ls;			
		public Color	color;
		public Vector3	dir;		// direction the light comes from

		public void scale_radiance(float b)
		{
			ls = b;
		}

		public void set_color(Color c)
		{
			color  = c; 
		}

		public void set_direction(Vector3 d)
		{
			dir = d;
			dir.Normalize ();
		}
		
		public override Vector3	get_direction(ref Shade s)
		{
			return -dir;
		}

		public override Color L(ref Shade s)
		{
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
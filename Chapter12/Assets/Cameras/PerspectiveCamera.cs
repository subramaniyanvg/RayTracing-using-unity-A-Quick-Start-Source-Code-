using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveCamera : Camra 
{
	public float	d;		// view plane distance
	public float	zoom;	// zoom factor

	public PerspectiveCamera():base()
	{
		d = 500.0f;
		zoom = 1.0f;
	}

	public void set_view_distance(float vpd)
	{
		d = vpd;
	}

	public void set_zoom(float zoom_factor)
	{
		zoom = zoom_factor;
	}

	public Vector3	get_direction(Vector2 p)
	{
		Vector3 dir = p.x * u + p.y * v - d * w;
		dir.Normalize();

		return(dir);
	}

	public override void render_scene(World w)
	{
		Color	L = Color.black;
		ViewPlane	vp = w.vp;	 								
		Ray			ray = new Ray();
		Vector2 	pp;		// sample point on a pixel
		int n = (int)Mathf.Sqrt((float)vp.num_samples);

		vp.s /= zoom;
		ray.origin = eye;

		for (int r = 0; r < vp.vres; r++) // up
		{
			for (int c = 0; c < vp.hres; c++) // across
			{		 					
				L = Color.black; 
				for (int p = 0; p < n; p++)			// up pixel
					for (int q = 0; q < n; q++) 
					{	// across pixel
						pp.x = vp.s * (c - 0.5f * vp.hres + (q + 0.5f) / n); 
						pp.y = vp.s * (r - 0.5f * vp.vres + (p + 0.5f) / n);
						ray.direction = get_direction (pp);
						L += w.tracer_ptr.trace_ray (ray);
					}	

				L /= vp.num_samples;
				L *= exposure_time;
				w.display_pixel (r, c, L);
			}
		}
		w.texture.Apply ();
	}
}
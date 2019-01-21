using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicCamera : Camra 
{
	public override void render_scene(World w)
	{
		Color		pixel_color;	 	
		Ray			ray = new Ray();
		ViewPlane vp = w.vp;
		int 		hres 	= vp.hres;
		int 		vres 	= vp.vres;
		float		s		= vp.s;
		float		zw		= 100.0f;				// hardwired in
		Vector2 	pp;
		Vector2		sp;
		ray.direction = new Vector3(0, 0, -1);

		for (int r = 0; r < vres; r++)			// up
		{
			for (int c = 0; c <= hres; c++) // across
			{	 	
				pixel_color = Color.black;
				for(int j = 0 ; j < vp.num_samples ; j++)
				{
					sp =  vp.sample_ptr.sample_unit_square();
					pp.x = vp.s * (c - (0.5f * vp.hres) + sp.x);
					pp.y = vp.s * (r - (0.5f * vp.vres) + sp.y);
					ray.origin = new Vector3(pp.x,pp.y, zw);
					pixel_color += w.tracer_ptr.trace_ray(ray);
				}
				pixel_color /= vp.num_samples;
				w.display_pixel(r, c, pixel_color);
			}	
		}
		w.texture.Apply ();
	}
}
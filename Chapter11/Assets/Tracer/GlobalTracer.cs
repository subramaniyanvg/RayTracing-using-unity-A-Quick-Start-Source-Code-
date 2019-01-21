using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTracer : Tracer 
{
	public GlobalTracer ()
	{
	}

	public GlobalTracer(World world)
	{
		world_ptr = world;
	}

	public override Color trace_ray(Ray ray)
	{
		Shade sr = null;
		sr = world_ptr.hit_objects(ray);
		int depth = 0;
		if (sr.hit_an_object)
		{
			sr.depth = depth;
			sr.ray = ray;			// used for specular shading
			return (sr.material_ptr.global_shade(ref sr));
		}   
		else
			return (world_ptr.background_color);
	}

	public override Color trace_ray(Ray ray,int depth)
	{
		if (depth > world_ptr.vp.max_depth)
			return Constants.black;
		else
		{
			Shade sr = null;
			sr = world_ptr.hit_objects (ray);
			if (sr.hit_an_object)
			{
				sr.depth = depth;
				sr.ray = ray;			// used for specular shading
				return (sr.material_ptr.global_shade (ref sr));
			} 
			else
				return (world_ptr.background_color);
		}
	}
}
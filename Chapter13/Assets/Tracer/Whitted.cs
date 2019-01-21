using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whitted : Tracer 
{
	public Whitted ()
	{
	}

	public Whitted(World world)
	{
		world_ptr = world;
	}

	public override Color trace_ray(Ray ray)
	{
		Shade sr = world_ptr.hit_objects(ray);
		int depth = 0;
		if (sr.hit_an_object) 
		{
			sr.depth = depth;
			sr.ray = ray;			
			return (sr.material_ptr.shade (ref sr));
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
			Shade sr = world_ptr.hit_objects(ray);
			if (sr.hit_an_object) 
			{
				sr.depth = depth;
				sr.ray = ray;			
				return (sr.material_ptr.shade (ref sr));
			} 
			else
				return (world_ptr.background_color);
		}
	}

	public override Color trace_ray(Ray ray,ref float tMin,int depth)
	{
		if (depth > world_ptr.vp.max_depth)
			return Constants.black;
		else
		{
			Shade sr = world_ptr.hit_objects(ray);
			if (sr.hit_an_object) 
			{
				sr.depth = depth;
				sr.ray = ray;
				tMin = sr.t;
				return (sr.material_ptr.shade (ref sr));
			} 
			else 
			{
				tMin = Constants.kHugeValue;
				return (world_ptr.background_color);
			}
		}
	}
}
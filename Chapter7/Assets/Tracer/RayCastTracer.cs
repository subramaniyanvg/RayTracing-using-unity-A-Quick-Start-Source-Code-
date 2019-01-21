using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTracer : Tracer 
{
	public RayCastTracer ()
	{
	}

	public RayCastTracer(World world)
	{
		world_ptr = world;
	}

	public override Color trace_ray(Ray ray)
	{
		Shade sr = null;
		sr = world_ptr.hit_objects(ray);
		if (sr.hit_an_object)
			return Color.red;
		else
			return (world_ptr.background_color);
	}

	public override Color trace_ray(Ray ray,int depth)
	{
		Shade sr = null;
		sr = world_ptr.hit_objects(ray);
		if (sr.hit_an_object)
			return Color.red;
		else
			return (world_ptr.background_color);
	}
}
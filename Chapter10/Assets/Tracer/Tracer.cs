﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracer 
{
	public virtual Color trace_ray(Ray ray)
	{
		return Constants.black;
	}

	public virtual Color trace_ray(Ray ray,int depth)
	{
		return Constants.black;
	}
	public World world_ptr;
}
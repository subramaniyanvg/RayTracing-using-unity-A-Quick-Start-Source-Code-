using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : Lighting 
{
	public MeshObject obj_ptr;
	public Matrial 	  materail_ptr;
	public Vector3 	  sample_point;
	public Vector3 	  light_normal;
	public Vector3	  wi;

	public override Vector3	get_direction(ref Shade s)
	{
		sample_point = obj_ptr.sample ();
		light_normal = obj_ptr.get_normal (ref sample_point);
		wi = sample_point - s.hit_point;
		wi.Normalize ();
		return wi;
	}

	public override Color L(ref Shade s)
	{
		float ndotd = Vector3.Dot (-light_normal, wi);
		if (ndotd > 0)
			return materail_ptr.get_le ();
		return Constants.black;
	}

	public override float G(ref Shade s)
	{
		float ndotd =  Vector3.Dot(-light_normal, wi);
		float d2 = (sample_point.magnitude) * (sample_point.magnitude);
		return ndotd/d2;
	}

	public override float pdf(ref Shade s)
	{
		return obj_ptr.pdf(ref s);
	}

	public virtual void set_object(MeshObject objPtr)
	{
		obj_ptr = objPtr;
		materail_ptr = obj_ptr.get_material ();
	}
}
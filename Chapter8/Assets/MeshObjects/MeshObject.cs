using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshObject 
{
	public virtual bool hit(Ray ray,ref float t,ref Shade s)
	{
		t = Mathf.Infinity;
		s = null;
		return false;
	}

	public Matrial	get_material()
	{
		return (material_ptr);
	}

	public virtual void set_material(Matrial mPtr)
	{
		material_ptr = mPtr;
	}

	public virtual Vector3 sample()
	{
		return Vector3.positiveInfinity;
	}

	public virtual float pdf(ref Shade s)
	{
		return 1.0f;
	}

	public virtual Vector3 get_normal(ref Vector3 p)
	{
		return Vector3.positiveInfinity;
	}

	Matrial   material_ptr;
}
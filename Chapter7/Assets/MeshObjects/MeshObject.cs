using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshObject 
{
	public virtual bool hit(Ray ray,ref float t,ref Shade s)
	{
		return false;
	}
}
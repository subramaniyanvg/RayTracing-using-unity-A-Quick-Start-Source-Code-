using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MeshObject
{
	public Vector3 planeNormal = new Vector3 (0, 0, -1);
	public Vector3 planePassThrghPnt = new Vector3 (0, 0, 1);

	public override bool hit(Ray ray,ref float t,ref Shade s)
	{
		t = Vector3.Dot((planePassThrghPnt - ray.origin),planeNormal) / Vector3.Dot(ray.direction,planeNormal);
		if (t >= Constants.kEpsilon) 
		{
			Vector3 point = ray.origin + t * ray.direction;
			s.normal = planeNormal;
			s.local_hit_point = point;
			return true;
		}
		return false;
	}

	public override bool shadow_hit(ref Ray ray,ref float tMin)
	{
		float t = Vector3.Dot((planePassThrghPnt - ray.origin),planeNormal) / Vector3.Dot(ray.direction,planeNormal);
		if (t > Constants.kEpsilon) 
		{
			tMin = t;
			return true;
		}
		return false;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MeshObject {

	public Vector3 circleNormal = new Vector3 (0, 0, -1);
	public Vector3 circleCenter = new Vector3 (0, 0, 0);
	public float circleRad = 85;

	public override bool hit(Ray ray,ref float t,ref Shade s)
	{
		t  = Vector3.Dot ((circleCenter - ray.origin), circleNormal) / Vector3.Dot (ray.direction, circleNormal);
		if (t >= Constants.kEpsilon)
		{
			Vector3 point = ray.origin + (float)t * ray.direction;
			float distance = Vector3.Distance (point, circleCenter);
			if ((distance * distance) <= (circleRad * circleRad)) 
			{
				s.normal = circleNormal;
				s.local_hit_point = point;
				return true;
			}
		}
		return false;
	}
}
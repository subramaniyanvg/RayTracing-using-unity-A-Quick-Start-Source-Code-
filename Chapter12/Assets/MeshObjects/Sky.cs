using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MeshObject
{
	public Vector3 sphereCenter;
	public float   sphereRad;
	public override bool hit(ref Ray ray,ref float t,ref Shade s)
	{
		Vector3 temp = ray.origin - sphereCenter;
		float a = Vector3.Dot(ray.direction,ray.direction);
		float b = (2.0f * Vector3.Dot(temp,ray.direction));
		float c =  Vector3.Dot(temp,temp) - (sphereRad * sphereRad);
		float d = b * b - 4.0f * a * c;
		if(d >= 0)
		{
			float e = Mathf.Sqrt ((float)d);
			float denom = 2.0f * a;
			t = (-b - e) / denom;//smaller root
			Vector3 hitPoint = Vector3.positiveInfinity;
			Vector3 normal = Vector3.positiveInfinity;
			if (t > Constants.kEpsilon) 
			{
				s.normal = -(temp + (float)t*ray.direction) / sphereRad;
				s.local_hit_point = ray.origin + (float)t * ray.direction;
				return true;
			}
			t = (-b + e) / denom;//larger root
			if (t > Constants.kEpsilon) 
			{
				s.normal = -(temp + (float)t*ray.direction) / sphereRad;
				s.local_hit_point = ray.origin + (float)t * ray.direction;
				return true;
			}
		}
		return false;
	}
}
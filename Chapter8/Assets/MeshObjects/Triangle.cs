﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle: MeshObject 
{
	Vector3 triangleNormal = Vector3.zero;
	public Vector3 v0 = new Vector3 (0, 0, 0);
	public Vector3 v1 = new Vector3 (50, 10, 0);
	public Vector3 v2 = new Vector3 (25, 50, 0);

	public Triangle(Vector3 v0,Vector3 v1,Vector3 v2)
	{
		this.v0 = v0;
		this.v1 = v1;
		this.v2 = v2;
		triangleNormal = (Vector3.Cross ((this.v1 - this.v0), (this.v2 - this.v0)) / Vector3.Magnitude (Vector3.Cross ((this.v1 - this.v0), (this.v2 - this.v0)))).normalized;
	}

	public override bool hit(Ray ray,ref float t,ref Shade s)
	{
		double a = v0.x - v1.x, b = v0.x - v2.x, c = ray.direction.x, d = v0.x - ray.origin.x;
		double e = v0.y - v1.y, f = v0.y - v2.y, g = ray.direction.y, h = v0.y - ray.origin.y;
		double i = v0.z - v1.z, j = v0.z - v2.z, k = ray.direction.z, l = v0.z - ray.origin.z;

		double m = f * k - g * j, n = g * l - h * k, o = h * j - f * l;
		double p = h * k - g * l, q = g * i - e * k , r = e*l - h * i;
		double snew = f * l - h * j, tnew = h * i - e * l , u = e*j - f * i;

		double inv_demnom =  a * m + b * q + c * u;
		double beta = (d * m + b * n + c * o) / inv_demnom;
		double gamma = (a * p + d * q + c * r) / inv_demnom;
		double tVal = (a * snew + b * tnew + d * u) / inv_demnom;

		if (beta >= 0 && gamma >= 0 && beta + gamma <= 1 && tVal >= Constants.kEpsilon) 
		{
			Vector3 hitPoint = ray.origin + ((float)tVal * ray.direction);
			t = (float)tVal;
			s.normal = triangleNormal;
			s.local_hit_point = hitPoint;
			return true;
		}
		return false;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle: MeshObject 
{
	public Vector3 triangleNormal = Vector3.zero;
	public Vector3 v0 = new Vector3 (0, 0, 0);
	public Vector2 v0_uv = new Vector2 (0, 0);

	public Vector3 v1 = new Vector3 (50, 10, 0);
	public Vector2 v1_uv = new Vector2 (0, 0);

	public Vector3 v2 = new Vector3 (25, 50, 0);
	public Vector2 v2_uv = new Vector2 (0, 0);

	public Triangle(Vector3 v0,Vector3 v1,Vector3 v2,Vector2 v0_uv,Vector2 v1_uv,Vector2 v2_uv,bool computeNormal)
	{
		this.v0 = v0;
		this.v1 = v1;
		this.v2 = v2;

		this.v0_uv.x = v0_uv.x;
		this.v0_uv.y = v0_uv.y;

		this.v1_uv.x = v1_uv.x;
		this.v1_uv.y = v1_uv.y;

		this.v2_uv.x = v2_uv.x;
		this.v2_uv.y = v2_uv.y;

        if (computeNormal)
        {
            triangleNormal = (Vector3.Cross((this.v1 - this.v0), (this.v2 - this.v0)) / Vector3.Magnitude(Vector3.Cross((this.v1 - this.v0), (this.v2 - this.v0)))).normalized;
            triangleNormal = -triangleNormal;
        }

    }

	public override bool hit(ref Ray ray,ref float t,ref Shade s)
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
			Vector2 uvCoordinates = interpolate_uv ((float)beta, (float)gamma);
			s.u = uvCoordinates.x;
			s.v = uvCoordinates.y;
			return true;
		}
		return false;
	}

	public Vector2 interpolate_uv(float beta,float gamma)
	{
		Vector2 uv = (1 - beta - gamma) * v0_uv + beta * v1_uv + gamma * v2_uv;
		return uv;
	}

	public override bool shadow_hit(ref Ray ray,ref float tMin)
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
		double t = (a * snew + b * tnew + d * u) / inv_demnom;

		if (beta >= 0 && gamma >= 0 && beta + gamma <= 1 && t >= Constants.kEpsilon) 
		{
			Vector3 hitPoint = ray.origin + ((float)t * ray.direction);
			tMin = (float)t;
			return true;
		}
		return false;
	}
}
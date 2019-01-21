using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangular : MeshObject
{
	public Vector3 rectBotLeftPnt;
	public Vector3 rectBotRightPnt;
	public Vector3 rectTopLeftPnt;
	public Vector3 normal;
	public Sampler sampler_ptr;
	public float inv_area;

	public Rectangular(Vector3 rectBotLeftPnt,Vector3 rectBotRightPnt,Vector3 rectTopLeftPnt)
	{
		inv_area = 1.0f/((rectBotRightPnt - rectBotLeftPnt).magnitude * (rectTopLeftPnt - rectBotLeftPnt).magnitude);
		this.rectBotLeftPnt = rectBotLeftPnt;
		this.rectBotRightPnt = rectBotRightPnt;
		this.rectTopLeftPnt = rectTopLeftPnt;
		normal = -(Vector3.Cross ((rectTopLeftPnt-rectBotLeftPnt),(rectBotRightPnt - rectBotLeftPnt)) / Vector3.Magnitude (Vector3.Cross ((rectTopLeftPnt-rectBotLeftPnt),(rectBotRightPnt - rectBotLeftPnt)))).normalized;
	}


	public void Set_Sampler(Sampler sampler)
	{
		sampler_ptr = sampler;
	}

	public override Vector3 sample()
	{
		Vector2 samplePoint = sampler_ptr.sample_unit_square ();
		return (rectBotLeftPnt + samplePoint.x * rectBotRightPnt + samplePoint.y * rectTopLeftPnt);
	}

	public override float pdf(ref Shade s)
	{
		return  inv_area;
	}

	public override Vector3 get_normal(ref Vector3 p)
	{
		return normal;
	}

	public override bool hit(ref Ray ray,ref float t,ref Shade s)
	{
		t = Vector3.Dot((rectBotLeftPnt - ray.origin),normal) / Vector3.Dot(ray.direction,normal);
		if (t >= Constants.kEpsilon) 
		{
			Vector3 point = ray.origin + t * ray.direction;
			Vector3 d = point - rectBotLeftPnt;
			float ddota = Vector3.Dot(d,(rectBotRightPnt - rectBotLeftPnt));
			float mag = Vector3.Magnitude (rectBotRightPnt - rectBotLeftPnt);
			if (ddota >= 0.0f && ddota <= (mag * mag))
			{
				ddota = Vector3.Dot (d, (rectTopLeftPnt - rectBotLeftPnt));
				mag = Vector3.Magnitude (rectTopLeftPnt - rectBotLeftPnt);
				if (ddota >= 0.0f && ddota <= (mag * mag)) 
				{
					s.local_hit_point = point;
					s.normal = normal;
					return true;
				}
			}
		}
		return false;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle : MeshObject
{
	public Vector3 rectBotLeftPnt = new Vector3 (0, 0, 0);
	public Vector3 rectBotRightPnt = new Vector3 (90, 0, 0);
	public Vector3 rectTopLeftPnt = new Vector3 (0, 90, 0);
	private Vector3 rectNormal = Vector3.positiveInfinity;

	public Rectangle(Vector3 rectBotLeftPnt,Vector3 rectBotRightPnt,Vector3 rectTopLeftPnt)
	{
		this.rectBotLeftPnt = rectBotLeftPnt;
		this.rectBotRightPnt = rectBotRightPnt;
		this.rectTopLeftPnt = rectTopLeftPnt;
		rectNormal = (Vector3.Cross ((rectTopLeftPnt-rectBotLeftPnt),(rectBotRightPnt - rectBotLeftPnt)) / Vector3.Magnitude (Vector3.Cross ((rectTopLeftPnt-rectBotLeftPnt),(rectBotRightPnt - rectBotLeftPnt)))).normalized;
	}

	public override bool hit(Ray ray,ref float t,ref Shade s)
	{
		t = Vector3.Dot((rectBotLeftPnt - ray.origin),rectNormal) / Vector3.Dot(ray.direction,rectNormal);
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
					s.normal = rectNormal;
					return true;
				}
			}
		}
		return false;
	}
}

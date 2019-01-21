using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle : MeshObject
{
	public Vector3 rectBotLeftPnt = new Vector3 (0, 0, 0);
	public Vector2 rectBotLeftPnt_uv = new Vector2 (0, 0);

	public Vector3 rectBotRightPnt = new Vector3 (90, 0, 0);
	public Vector2 rectBotRightPnt_uv = new Vector2 (0, 0);

	public Vector3 rectTopLeftPnt = new Vector3 (0, 90, 0);
	public Vector2 rectTopLeftPnt_uv = new Vector2 (0, 0);

	private Vector3 rectNormal = Vector3.positiveInfinity;

	public Rectangle(Vector3 rectBotLeftPnt,Vector3 rectBotRightPnt,Vector3 rectTopLeftPnt,Vector2 rectBotLeftPnt_uv,Vector2 rectBotRightPnt_uv,Vector2 rectTopLeftPnt_uv)
	{
		this.rectBotLeftPnt = rectBotLeftPnt;
		this.rectBotLeftPnt_uv = rectBotLeftPnt_uv;
	
		this.rectBotRightPnt = rectBotRightPnt;
		this.rectBotRightPnt_uv = rectBotRightPnt_uv;

		this.rectTopLeftPnt = rectTopLeftPnt;
		this.rectTopLeftPnt_uv = rectTopLeftPnt_uv;

		rectNormal = (Vector3.Cross ((rectTopLeftPnt-rectBotLeftPnt),(rectBotRightPnt - rectBotLeftPnt)) / Vector3.Magnitude (Vector3.Cross ((rectTopLeftPnt-rectBotLeftPnt),(rectBotRightPnt - rectBotLeftPnt)))).normalized;
	}

	public override bool hit(ref Ray ray,ref float t,ref Shade s)
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
					Vector2 uv = interpolate_uv (point);
					s.u = uv.x;
					s.v = uv.y;
					return true;
				}
			}
		}
		return false;
	}

	public Vector2 interpolate_uv(Vector3 hitPoint)
	{
		Vector2 uv = Vector2.zero;

		float hitPointMAgFromBottomLeft = Vector3.Magnitude ((hitPoint - rectBotLeftPnt));
		hitPoint = (hitPoint - rectBotLeftPnt).normalized * hitPointMAgFromBottomLeft;

		Vector3 uVector = Vector3.Project (hitPoint, (rectBotRightPnt - rectBotLeftPnt).normalized);
		float rightVectorMagnitude = Vector3.Magnitude(rectBotRightPnt - rectBotLeftPnt);
		float uVectorMagnitude = Vector3.Magnitude(uVector);
		uv.x = Mathf.Lerp (0, 1, uVectorMagnitude / rightVectorMagnitude);

		Vector3 vVector = Vector3.Project (hitPoint, (rectTopLeftPnt - rectBotLeftPnt).normalized);
		float upVectorMagnitude = Vector3.Magnitude(rectTopLeftPnt - rectBotLeftPnt);
		float vVectorMagnitude = Vector3.Magnitude (vVector);
		uv.y = Mathf.Lerp (0, 1, vVectorMagnitude / upVectorMagnitude);

		return uv;
	}

	public override bool shadow_hit(ref Ray ray,ref float tMin)
	{
		float t = Vector3.Dot((rectBotLeftPnt - ray.origin),rectNormal) / Vector3.Dot(ray.direction,rectNormal);
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
					tMin = t;
					return true;
				}
			}
		}
		return false;
	}
}

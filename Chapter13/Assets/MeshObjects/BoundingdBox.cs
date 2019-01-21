using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingdBox: MeshObject
{
	public Vector3  boxBotLeftBackPnt = new Vector3(0,0,0);
	public Vector3  boxTopRightFrontPnt = new Vector3(70,70,70);

	public override bool hit(ref Ray ray,ref float t,ref Shade s)
	{
		double ox = ray.origin.x, oy = ray.origin.y, oz = ray.origin.z;
		double dx = ray.direction.x, dy = ray.direction.y, dz = ray.direction.z;

		double tx_min = 0, ty_min = 0, tz_min = 0;
		double tx_max = 0, ty_max = 0, tz_max = 0;

		double a = 1.0f / dx;

		if (a >= 0) 
		{
			tx_min = (boxBotLeftBackPnt.x - ox) * a;
			tx_max = (boxTopRightFrontPnt.x - ox) * a;
		}
		else 
		{
			tx_min = (boxTopRightFrontPnt.x - ox) * a;
			tx_max = (boxBotLeftBackPnt.x - ox) * a;
		}

		double b = 1.0f / dy;

		if (b >= 0) 
		{
			ty_min = (boxBotLeftBackPnt.y - oy) * b;
			ty_max = (boxTopRightFrontPnt.y - oy) * b;
		}
		else 
		{
			ty_min = (boxTopRightFrontPnt.y - oy) * b;
			ty_max = (boxBotLeftBackPnt.y - oy) * b;
		}

		double c = 1.0f / dz;

		if (c >= 0) 
		{
			tz_min = (boxBotLeftBackPnt.z - oz) * c;
			tz_max = (boxTopRightFrontPnt.z - oz) * c;
		}
		else 
		{
			tz_min = (boxTopRightFrontPnt.y - oz) * c;
			tz_max = (boxBotLeftBackPnt.y - oz) * c;
		}

		double t0, t1;
		int face_in, face_out;
		if (tx_min > ty_min) {
			t0 = tx_min;
			face_in = (a >= 0) ? 0 : 3;
		} else {
			t0 = ty_min;
			face_in = (b >= 0) ? 1 : 4;
		}

		if (tz_min > t0) {
			t0 = tz_min;
			face_in = (c >= 0) ? 2 : 5;

		}
		if (tx_max < ty_max) {
			t1 = tx_max;
			face_out = (a >= 0) ? 3 : 0;
		} else {
			t1 = ty_max;
			face_out = (b >= 0) ? 4 : 1;
		}
		if (tz_max < t1) {
			t1 = tz_max;
			face_out = (c >= 0) ? 5 : 2;
		}
		if (t0 < t1 && t1 > Constants.kEpsilon) 
		{
			double tMin = 0;
			Vector3 normal = Vector3.zero;
			if (t0 > Constants.kEpsilon) 
			{
				tMin = t0;
				t = (float)t0;
				normal = GetNormal (face_in);
			} 
			else
			{
				tMin = t1;
				t = (float)t1;
				normal = GetNormal (face_out);
			}
			Vector3 hitPoint = Vector3.zero;
			hitPoint = new Vector3 ((float)ox, (float)oy, (float)oz) + ((float)tMin * ray.direction);
			s.local_hit_point = hitPoint;
			s.normal = normal;
			return true;
		}
		return false;
	}

	public override bool shadow_hit(ref Ray ray,ref float t)
	{
		double ox = ray.origin.x, oy = ray.origin.y, oz = ray.origin.z;
		double dx = ray.direction.x, dy = ray.direction.y, dz = ray.direction.z;

		double tx_min = 0, ty_min = 0, tz_min = 0;
		double tx_max = 0, ty_max = 0, tz_max = 0;

		double a = 1.0f / dx;

		if (a >= 0) 
		{
			tx_min = (boxBotLeftBackPnt.x - ox) * a;
			tx_max = (boxTopRightFrontPnt.x - ox) * a;
		}
		else 
		{
			tx_min = (boxTopRightFrontPnt.x - ox) * a;
			tx_max = (boxBotLeftBackPnt.x - ox) * a;
		}

		double b = 1.0f / dy;

		if (b >= 0) 
		{
			ty_min = (boxBotLeftBackPnt.y - oy) * b;
			ty_max = (boxTopRightFrontPnt.y - oy) * b;
		}
		else 
		{
			ty_min = (boxTopRightFrontPnt.y - oy) * b;
			ty_max = (boxBotLeftBackPnt.y - oy) * b;
		}

		double c = 1.0f / dz;

		if (c >= 0) 
		{
			tz_min = (boxBotLeftBackPnt.z - oz) * c;
			tz_max = (boxTopRightFrontPnt.z - oz) * c;
		}
		else 
		{
			tz_min = (boxTopRightFrontPnt.y - oz) * c;
			tz_max = (boxBotLeftBackPnt.y - oz) * c;
		}

		double t0, t1;
		if (tx_min > ty_min) 
			t0 = tx_min;
		else 
			t0 = ty_min;

		if (tz_min > t0)
			t0 = tz_min;
		if (tx_max < ty_max)
			t1 = tx_max;
		else
			t1 = ty_max;
		if (tz_max < t1)
			t1 = tz_max;
		if (t0 < t1 && t1 > Constants.kEpsilon) 
		{
			double tMin = 0;
			if (t0 > Constants.kEpsilon) 
			{
				tMin = t0;
				t = (float)t0;
			} 
			else
			{
				tMin = t1;
				t = (float)t1;
			}
			return true;
		}
		return false;
	}

	Vector3 GetNormal(int face)
	{
		switch(face)
		{
			case 0:
				return new Vector3 (-1, 0, 0);
			case 1:
				return new Vector3 (0, -1, 0);
			case 2:
				return new Vector3 (0, 0, -1);
			case 3:
				return new Vector3 (1, 0, 0);
			case 4:
				return new Vector3 (0, 1, 0);
			case 5:
				return new Vector3 (0, 0, 1);
		}
		return new Vector3 (Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
	}
}
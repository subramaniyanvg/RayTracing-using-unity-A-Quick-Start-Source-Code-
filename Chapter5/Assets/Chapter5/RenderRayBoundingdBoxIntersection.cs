using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRayBoundingdBoxIntersection: MonoBehaviour
{
	Vector3 eye = new Vector3 (0, 0, 500);//Camera position
	//Orthonormal basis vector
	Vector3 u = new Vector3(1,0,0);
	Vector3 v = Vector3.up;
	Vector3 w = new Vector3(0,0,1);

	float   d = 500;//ViewPlane Distance
	float epsilon =  0.0001f;
	Vector3 rayDir = Vector3.zero;
	Texture2D texture = null;
	public Vector3  boxBotLeftBackPnt = new Vector3(0,0,0);
	public Vector3  boxTopRightFrontPnt = new Vector3(70,70,70);


	// Use this for initialization
	void Start () {
		texture = new Texture2D(200,200);
		GetComponent<Renderer>().material.mainTexture = texture;
	}

	//Gets the ray direction from the given point
	Vector3 GetDirection(Vector2 point)
	{
		//Multiply the given point with "uv" vector and add them then multiply the "w" vector with d and then finally add these vector with the existing vector to get the resultant vector. 
		//Normalize the resultant vector to get the ray Direction.
		Vector3 dir = point.x * u + point.y * v - d * w;
		return dir.normalized;
	}

	// Update is called once per frame
	void Update () 
	{
		//As this is a perspective camera for all the ray directions that we compute below will have the rayOrigin equals to "eye".
		Vector3 rayOrigin = eye;
		//y = 0 means bottom left pixel.
		for (int y = 0; y < texture.height; y++)
		{
			//x = 0 means bottom left pixel.
			for (int x = 0; x < texture.width; x++)
			{
				Color color = Color.black;

				double ox = rayOrigin.x, oy = rayOrigin.y, oz = rayOrigin.z;
				float xPoint = x - (int)(0.5f * texture.width);
				float yPoint = y - (int)(0.5f * texture.height);
				rayDir = GetDirection (new Vector2 (xPoint, yPoint));
				double dx = rayDir.x, dy = rayDir.y, dz = rayDir.z;

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
					tz_min = (boxTopRightFrontPnt.z - oz) * c;
					tz_max = (boxBotLeftBackPnt.z - oz) * c;
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
				if (t0 < t1 && t1 > epsilon) 
				{
					double tMin = 0;
					Vector3 normal = Vector3.zero;
					if (t0 > epsilon) 
					{
						tMin = t0;
						normal = GetNormal (face_in);
					} 
					else
					{
						tMin = t1;
						normal = GetNormal (face_out);
					}
					Vector3 hitPoint = Vector3.zero;
					hitPoint = new Vector3 ((float)ox, (float)oy, (float)oz) + ((float)tMin * rayDir);
					color = Color.red;
				}
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
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
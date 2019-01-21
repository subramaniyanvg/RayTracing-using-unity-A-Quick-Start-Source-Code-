using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRayBoundingdBoxIntersection: MonoBehaviour
{
	float epsilon =  0.0001f;//Very small value closer to 0 with these value we do the intersection test 
	Vector3 rayDir = new Vector3 (0, 0, -1);//Direction of ray
	Texture2D texture = null;
	float rayOriginZDist = 100;//Ray Z dist we should always make sure that we shoot a ray from specific distance from the image if we shoot the ray from the image then there won't be any intersection.
	//Make sure boxTopRightFrontPnt is greater than boxBotLeftBackPnt in x,y and z coordinates else intersection will fail
	public Vector3  boxBotLeftBackPnt = new Vector3(100,100,0);
	public Vector3  boxTopRightFrontPnt = new Vector3(125,125,1);


	// Use this for initialization
	void Start () {
		texture = new Texture2D(200,200);
		GetComponent<Renderer>().material.mainTexture = texture;
	}

	// Update is called once per frame
	void Update () 
	{
		//y = 0 means bottom left pixel.
		for (int y = 0; y < texture.height; y++)
		{
			//x = 0 means bottom left pixel.
			for (int x = 0; x < texture.width; x++)
			{
				Color color = Color.black;

				double ox = x, oy = y, oz = rayOriginZDist;
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
				//If below condition is satisfied Color the pixel with red color else Color the pixel with black color
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
					hitPoint = new Vector3 (x, y, rayOriginZDist) + ((float)tMin * rayDir);
					color = Color.red;
				}
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
	}

	//Normals are harcoded for axis aligned bounding box
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
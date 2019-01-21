using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRaySphereIntersection : MonoBehaviour {

	float epsilon =  0.0001f;//Very small value closer to 0 with these value we do the intersection test
	Vector3 rayDir = new Vector3 (0, 0, -1);//Direction of ray
	Texture2D texture = null;
	float rayOriginZDist = 100;//Ray Z dist we should always make sure that we shoot a ray from specific distance from the image if we shoot the ray from the image then there won't be any intersection.
	public Vector3 sphereCenter = new Vector3 (100, 100, 50);
	public float sphereRad = 20;

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
				//The source code listed below is converted from the formula that was presented in last chapter
				Color color = Color.black;
				double t;
				Vector3 temp = new Vector3 (x, y, rayOriginZDist) - sphereCenter;
				double a = Vector3.Dot(rayDir,rayDir);
				double b = (double)(2.0f * Vector3.Dot(temp,rayDir));
				double c =  Vector3.Dot(temp,temp) - (sphereRad * sphereRad);
				double d = b * b - 4.0f * a * c;
				if(d >= 0)
				{
					double e = Mathf.Sqrt ((float)d);
					double denom = 2.0f * a;
					t = (-b - e) / denom;//smaller root
					Vector3 hitPoint = Vector3.positiveInfinity;
					Vector3 normal = Vector3.positiveInfinity;
					if (t > epsilon) 
					{
						normal = (temp + (float)t*rayDir) / sphereRad;
						hitPoint = new Vector3 (x, y, rayOriginZDist) + (float)t * rayDir;
						color = Color.red;
					}
					t = (-b + e) / denom;//larger root
					//If below condition is satisfied Color the pixel with red color else Color the pixel with black color
					if (t > epsilon) 
					{
						normal = (temp + (float)t*rayDir) / sphereRad;
						hitPoint = new Vector3 (x, y, rayOriginZDist) + (float)t * rayDir;
						color = Color.red;
					}
				}
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRaySphereIntersection : MonoBehaviour {

	Vector3 eye = new Vector3 (0, 0, 500);//Camera position
	//Orthonormal basis vector
	Vector3 u = new Vector3(1,0,0);
	Vector3 v = Vector3.up;
	Vector3 w = new Vector3(0,0,1);

	float   d = 500;//ViewPlane Distance

	float epsilon =  0.0001f;
	Vector3 rayDir = Vector3.zero;
	Texture2D texture = null;
	public Vector3 sphereCenter = new Vector3 (0, 0, 0);
	public float sphereRad = 20;

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
				double t;
				Vector3 temp = rayOrigin - sphereCenter;
				rayDir = GetDirection (new Vector2 (x, y));
				float xPoint = x - (int)(0.5f * texture.width);
				float yPoint = y - (int)(0.5f * texture.height);
				rayDir = GetDirection (new Vector2 (xPoint, yPoint));
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
						hitPoint = rayOrigin + (float)t * rayDir;
						color = Color.red;
					}
					t = (-b + e) / denom;//larger root
					if (t > epsilon) 
					{
						normal = (temp + (float)t*rayDir) / sphereRad;
						hitPoint = rayOrigin + (float)t * rayDir;
						color = Color.red;
					}
				}
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
	}
}
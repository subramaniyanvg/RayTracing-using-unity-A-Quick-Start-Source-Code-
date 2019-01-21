using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRayTraingleIntersection : MonoBehaviour {

	Vector3 eye = new Vector3 (0, 0, 500);//Camera position
	//Orthonormal basis vector
	Vector3 u = new Vector3(1,0,0);
	Vector3 v = Vector3.up;
	Vector3 w = new Vector3(0,0,1);

	float   d = 500;//ViewPlane Distance

	float epsilon =  0.0001f;
	Vector3 rayDir = Vector3.zero;
	Texture2D texture = null;
	Vector3 triangleNormal = Vector3.zero;
	public Vector3 v0 = new Vector3 (0, 0, 0);
	public Vector3 v1 = new Vector3 (50, 10, 0);
	public Vector3 v2 = new Vector3 (25, 50, 0);


	// Use this for initialization
	void Start () {
		texture = new Texture2D(200,200);
		GetComponent<Renderer>().material.mainTexture = texture;
		triangleNormal = (Vector3.Cross ((v1 - v0), (v2 - v0)) / Vector3.Magnitude (Vector3.Cross ((v1 - v0), (v2 - v0)))).normalized;
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
				float xPoint = x - (int)(0.5f * texture.width);
				float yPoint = y - (int)(0.5f * texture.height);
				rayDir = GetDirection (new Vector2 (xPoint, yPoint));
				double a = v0.x - v1.x, b = v0.x - v2.x, c = rayDir.x, d = v0.x - rayOrigin.x;
				double e = v0.y - v1.y, f = v0.y - v2.y, g = rayDir.y, h = v0.y - rayOrigin.y;
				double i = v0.z - v1.z, j = v0.z - v2.z, k = rayDir.z, l = v0.z - rayOrigin.z;

				double m = f * k - g * j, n = g * l - h * k, o = h * j - f * l;
				double p = h * k - g * l, q = g * i - e * k , r = e*l - h * i;
				double s = f * l - h * j, t = h * i - e * l , u = e*j - f * i;

				double inv_demnom =  a * m + b * q + c * u;
				double beta = (d * m + b * n + c * o) / inv_demnom;
				double gamma = (a * p + d * q + c * r) / inv_demnom;
				double tVal = (a * s + b * t + d * u) / inv_demnom;

				if (beta >= 0 && gamma >= 0 && beta + gamma <= 1 && tVal >= epsilon) 
				{
					Vector3 hitPoint = new Vector3 (512.0f, 512.0f, 0) + ((float)tVal * rayDir);
					color = Color.red;
				}
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
	}
}
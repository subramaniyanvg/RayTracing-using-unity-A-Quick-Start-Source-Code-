using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRayTraingleIntersection : MonoBehaviour {


	float epsilon =  0.0001f;//Very small value closer to 0 with these value we do the intersection test 
	Vector3 rayDir = new Vector3 (0, 0, -1);//Direction of ray
	Texture2D texture = null;
	Vector3 triangleNormal = Vector3.zero;
	float rayOriginZDist = 100;//Ray Z dist we should always make sure that we shoot a ray from specific distance from the image if we shoot the ray from the image then there won't be any intersection.
	public Vector3 v0 = new Vector3 (100, 100, 0);
	public Vector3 v1 = new Vector3 (135, 100, 0);
	public Vector3 v2 = new Vector3 (117, 120, 100);


	// Use this for initialization
	void Start () {
		texture = new Texture2D(200,200);
		GetComponent<Renderer>().material.mainTexture = texture;
		triangleNormal = (Vector3.Cross ((v1 - v0), (v2 - v0)) / Vector3.Magnitude (Vector3.Cross ((v1 - v0), (v2 - v0)))).normalized;
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
				//The source code listed below is taken from the formula that we listed in last chapter only thing that we did here is we converted the formula into computer understandable format.
				Color color = Color.black;
				double a = v0.x - v1.x, b = v0.x - v2.x, c = rayDir.x, d = v0.x - x;
				double e = v0.y - v1.y, f = v0.y - v2.y, g = rayDir.y, h = v0.y - y;
				double i = v0.z - v1.z, j = v0.z - v2.z, k = rayDir.z, l = v0.z - rayOriginZDist;

				double m = f * k - g * j, n = g * l - h * k, o = h * j - f * l;
				double p = h * k - g * l, q = g * i - e * k , r = e*l - h * i;
				double s = f * l - h * j, t = h * i - e * l , u = e*j - f * i;

				double inv_demnom =  a * m + b * q + c * u;
				double beta = (d * m + b * n + c * o) / inv_demnom;
				double gamma = (a * p + d * q + c * r) / inv_demnom;
				double tVal = (a * s + b * t + d * u) / inv_demnom;
				//If below condition is satisfied Color the pixel with red color else Color the pixel with black color
				if (beta >= 0 && gamma >= 0 && beta + gamma <= 1 && tVal >= epsilon) 
				{
					Vector3 hitPoint = new Vector3 (x, y, rayOriginZDist) + ((float)tVal * rayDir);
					color = Color.red;
				}
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
	}
}

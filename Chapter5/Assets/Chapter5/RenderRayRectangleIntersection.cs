using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRayRectangleIntersection : MonoBehaviour
{
	Vector3 eye = new Vector3 (0, 0, 500);//Camera position
	//Orthonormal basis vector
	Vector3 u = new Vector3(1,0,0);
	Vector3 v = Vector3.up;
	Vector3 w = new Vector3(0,0,1);

	float   d = 500;//ViewPlane Distance


	float epsilon =  0.0001f;
	Vector3 rayDir = Vector3.zero;
	private Vector3 rectNormal = Vector3.zero;
	Texture2D texture = null;
	public Vector3 rectBotLeftPnt = new Vector3 (0, 0, 0);
	public Vector3 rectBotRightPnt = new Vector3 (90, 0, 0);
	public Vector3 rectTopLeftPnt = new Vector3 (0, 90, 0);


	// Use this for initialization
	void Start () {
		texture = new Texture2D(200,200);
		GetComponent<Renderer>().material.mainTexture = texture;
		rectNormal = (Vector3.Cross ((rectTopLeftPnt-rectBotLeftPnt),(rectBotRightPnt - rectBotLeftPnt)) / Vector3.Magnitude (Vector3.Cross ((rectTopLeftPnt-rectBotLeftPnt),(rectBotRightPnt - rectBotLeftPnt)))).normalized;
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
				float t = Vector3.Dot((rectBotLeftPnt - rayOrigin),rectNormal) / Vector3.Dot(rayDir,rectNormal);
				if (t >= epsilon) 
				{
					Vector3 point = new Vector3 (rayOrigin.x, rayOrigin.y, rayOrigin.z) + t * rayDir;
					Vector3 d = point - rectBotLeftPnt;
					float ddota = Vector3.Dot(d,(rectBotRightPnt - rectBotLeftPnt));
					float mag = Vector3.Magnitude (rectBotRightPnt - rectBotLeftPnt);
					if (ddota >= 0.0f && ddota <= (mag * mag))
					{
						ddota = Vector3.Dot (d, (rectTopLeftPnt - rectBotLeftPnt));
						mag = Vector3.Magnitude (rectTopLeftPnt - rectBotLeftPnt);
						if (ddota >= 0.0f && ddota <= (mag * mag)) 
						{
							point = point;
							rectNormal = rectNormal;
							color = Color.red;
						}
					}
				}
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
	}
}
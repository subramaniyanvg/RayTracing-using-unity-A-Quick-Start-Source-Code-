using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRayRectangleIntersection : MonoBehaviour
{
	float epsilon =  0.0001f;//Very small value closer to 0 with these value we do the intersection test 
	Vector3 rayDir = new Vector3 (0, 0, -1);//Direction of ray
	private Vector3 rectNormal = Vector3.zero;
	Texture2D texture = null;
	float rayOriginZDist = 100;//Ray Z dist we should always make sure that we shoot a ray from specific distance from the image if we shoot the ray from the image then there won't be any intersection.
	public Vector3 rectBotLeftPnt = new Vector3 (100, 100, 0);
	public Vector3 rectBotRightPnt = new Vector3 (125, 100, 0);
	public Vector3 rectTopLeftPnt = new Vector3 (100, 125, 0);


	// Use this for initialization
	void Start () {
		texture = new Texture2D(200,200);
		GetComponent<Renderer>().material.mainTexture = texture;
		rectNormal = (Vector3.Cross ((rectTopLeftPnt-rectBotLeftPnt),(rectBotRightPnt - rectBotLeftPnt)) / Vector3.Magnitude (Vector3.Cross ((rectTopLeftPnt-rectBotLeftPnt),(rectBotRightPnt - rectBotLeftPnt)))).normalized;
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
				Vector3 rayOrigin = new Vector3 (x,y,rayOriginZDist);
				//Get value of t by taking dot product of rectNormal and (rectBotLeftPnt - rayOrigin) and divide it by the dot product of (rayDir,rectNormal)
				float t = Vector3.Dot((rectBotLeftPnt - rayOrigin),rectNormal) / Vector3.Dot(rayDir,rectNormal);
				//if t > epsilon it means there is a intersection
				if (t >= epsilon) 
				{
					//Find the intersection point 
					Vector3 point = new Vector3 (x, y, rayOriginZDist) + t * rayDir;
					//Get the direction by subtracting (point - rectBotLeftPnt) 
					Vector3 d = point - rectBotLeftPnt;
					//project "d" vector onto "a" vector  
					float ddota = Vector3.Dot(d,(rectBotRightPnt - rectBotLeftPnt));
					//Get the magnitude of "a" vector
					float mag = Vector3.Magnitude (rectBotRightPnt - rectBotLeftPnt);
					//If projectedVector is >= 0 and its magnitude is <= "a" vector magnitude squared then point lies on "a" vector direction
					if (ddota >= 0.0f && ddota <= (mag * mag))
					{
						//project "d" vector onto "b" vector  
						float ddotb = Vector3.Dot (d, (rectTopLeftPnt - rectBotLeftPnt));
						//Get the magnitude of "b" vector
						mag = Vector3.Magnitude (rectTopLeftPnt - rectBotLeftPnt);
						//If projectedVector is >= 0 and its magnitude is <= "b" vector magnitude squared then point lies on "b" vector direction and we can color that pixel with red color else set that pixel color to black. 
						if (ddotb >= 0.0f && ddotb <= (mag * mag)) 
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRayPlaneIntersection : MonoBehaviour {

	float epsilon =  0.0001f;//Very small value closer to 0 with these value we do the intersection test 
	Vector3 rayDir = new Vector3 (0, 0, -1);//Direction of ray
	Texture2D texture = null;
	float rayOriginZDist = 100;//Ray Z dist we should always make sure that we shoot a ray from specific distance from the image if we shoot the ray from the image then there won't be any intersection.  
	public Vector3 planeNormal = new Vector3 (0, 1, 0);
	public Vector3 planePassThrghPnt = new Vector3 (100, 100, 0);

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
				Vector3 rayOrigin = new Vector3 (x,y,rayOriginZDist);
				//Get value of t by taking dot product of planeNormal and (planePassThrghPnt - rayOrigin) and divide it by the dot product of (rayDir,planeNormal) 
				float t = Vector3.Dot((planePassThrghPnt - rayOrigin),planeNormal) / Vector3.Dot(rayDir,planeNormal);
				//if t > epsilon color that pixel with red color else set that pixel color to black
				if (t > epsilon) 
				{
					Vector3 point = new Vector3 (x, y, rayOriginZDist) + t * rayDir;
					planeNormal = planeNormal;
					color = Color.red;
				}
				texture.SetPixel(x,y,color);
			}
		}
		texture.Apply();
	}
}
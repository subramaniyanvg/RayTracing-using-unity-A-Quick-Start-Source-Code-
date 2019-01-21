using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRedColor : MonoBehaviour {

	Texture2D texture = null;

	// Use this for initialization
	void Start () {
		texture = new Texture2D(200,200);
		GetComponent<Renderer>().material.mainTexture = texture;
	}
	
	// Update is called once per frame
	void Update () {

		//y = 0 means bottom left pixel.
		for (int y = 0; y < texture.height; y++)
		{
			//x = 0 means bottom left pixel.
			for (int x = 0; x < texture.width; x++)
			{
				Color color = Color.red;
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
	}
}
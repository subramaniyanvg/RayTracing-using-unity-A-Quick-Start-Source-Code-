using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureData
{
	public Color 		color;
	public int 	 		hres;
	public int   		vres;
	public Color[,] 	texelColors;
	public TextureMapping	mapping = null;

	public void SetTextureMapping(TextureMapping map)
	{
		mapping = map;
	}

	public void LoadTextureData(Texture2D text)
	{
		hres = text.width;
		vres = text.height;
		texelColors = null;
		texelColors = new Color[text.width, text.height];

		for(int i = 0 ; i < text.width ; i++)
		{
			for (int j = 0; j < text.height; j++) 
				texelColors [i, j] = text.GetPixel (i, j);
		}		
	}

	public void set_color(Color c)
	{
		color = c;
	}

	public virtual Color get_constant_color()
	{
		return color;
	}

	public virtual Color get_color(ref Shade sr)
	{
		int row =  -1, column = -1;
		if (mapping != null)
			mapping.get_texel_coordinates (ref sr.local_hit_point, hres, vres, ref row, ref column);
		else 
		{
			row = (int)(sr.u * (hres - 1));
			column = (int)(sr.v * (vres - 1));
		}
		if (row >= hres && column >= vres)
			return Constants.black;
		return texelColors[row,column];
	}
}
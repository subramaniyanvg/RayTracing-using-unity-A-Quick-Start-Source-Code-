using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangularMapping : TextureMapping 
{
	//Will work only for unity sqaure that is placed at origin
	public override void get_texel_coordinates(ref Vector3 local_hit_point,int hRes,int vRes,ref int row,ref int column)
	{
		float u = local_hit_point.x;
		float v = local_hit_point.y;
		row = (int)((hRes - 1) * u);
		column = (int)((vRes - 1) * v);
	}
}
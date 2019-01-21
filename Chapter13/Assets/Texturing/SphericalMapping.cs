using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalMapping : TextureMapping 
{
	public override void get_texel_coordinates(ref Vector3 local_hit_point,int hRes,int vRes,ref int row,ref int column)
	{
		float theta = Mathf.Acos (local_hit_point.y);
		float phi = Mathf.Atan2 (local_hit_point.x, local_hit_point.z);
		if (phi < 0)
			phi += Constants.TWO_PI;
		float u = phi * Constants.invTWO_PI;
		float v = 1.0f - theta * Constants.invPI;

		row = (int)((hRes - 1) * u);
		column = (int)((vRes - 1) * v);
	}
}
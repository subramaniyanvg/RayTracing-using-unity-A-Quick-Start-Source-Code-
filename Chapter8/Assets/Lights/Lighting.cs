using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting {


	public virtual Vector3	get_direction(ref Shade s)
	{
		return Vector3.zero;
	}

	public virtual Color L(ref Shade s)
	{
		return new Color(0,0,0,1);
	}

	public virtual float G(ref Shade s)
	{
		return 1.0f;
	}

	public virtual float pdf(ref Shade s)
	{
		return 1.0f;
	}
}
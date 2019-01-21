using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrial  
{
	public Matrial()
	{
	}

	public virtual Color shade(ref Shade sr)
	{
		return Constants.black;
	}

	public virtual Color area_light_shade(ref Shade sr)
	{
		return Constants.black;
	}

	public virtual Color get_le()
	{
		return Constants.black;
	}
}
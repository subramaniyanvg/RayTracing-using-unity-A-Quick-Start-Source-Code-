using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants 
{
	public static float		PI
	{
		get 
		{
			return Mathf.PI;
		}

	}
	public static float		TWO_PI
	{
		get 
		{
			return Mathf.PI * 2.0f;
		}

	}
	public static float 	PI_ON_180 	= 0.0174532925199432957f;
	public static float 	invPI 		= 0.3183098861837906715f;
	public static float 	invTWO_PI 	= 0.1591549430918953358f;
	public static float 	kEpsilon 	= 0.0001f; 
	public static float		kHugeValue	= 1.0E10f;
	public static Color		black = new Color(0,0,0,1);
	public static Color		white = new Color(1,1,1,1);
	public static Color		red = new Color(1,0,0,1);
	public static float 	invRAND_MAX = 1.0f / (float)int.MaxValue;
}
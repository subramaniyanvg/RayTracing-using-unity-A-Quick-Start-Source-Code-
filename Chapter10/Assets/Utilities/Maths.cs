using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maths  
{
	public static int rand_int() 
	{
		return(Random.Range(0,int.MaxValue));
	}

	public static float rand_float() 
	{
		return((float)rand_int() * Constants.invRAND_MAX);
	}

	public static float rand_float(int l, float h) 
	{
		return (rand_float() * (h - l) + l);
	}

	public static int rand_int(int l, int h) 
	{
		return ((int) (rand_float(0, h - l + 1) + l));
	}

	public static double clamp(double x, double min, double max)
	{
		return (x < min ? min : (x > max ? max : x));
	}
}
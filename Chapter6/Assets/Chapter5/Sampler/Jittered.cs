using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jittered : Sampler 
{

	public override void generate_samples ()
	{
		int n = (int) Mathf.Sqrt((float)num_samples); 

		for (int p = 0; p < num_sets; p++)
			for (int j = 0; j < n; j++)		
				for (int k = 0; k < n; k++) {
					Vector2 sp = new Vector2 ((k + Rand_float()) / n, (j + Rand_float()) / n);				
					samples.Add(sp);
				}		
	}

	float Rand_float()
	{
		return ((float)Random.Range (0, int.MaxValue) * (1.0f / (float)int.MaxValue));
	}
}

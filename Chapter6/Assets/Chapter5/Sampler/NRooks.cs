using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NRooks : Sampler {

	public override void generate_samples ()
	{
		for (int p = 0; p < num_sets; p++)          			
			for (int j = 0; j < num_samples; j++) 
			{
				Vector2 sp = new Vector2((j + Rand_float()) / num_samples, (j + Rand_float()) / num_samples);
				samples.Add(sp);
			}		

		shuffle_x_coordinates();
		shuffle_y_coordinates();
	}

	float Rand_float()
	{
		return ((float)Random.Range (0, int.MaxValue) * (1.0f / (float)int.MaxValue));
	}
}

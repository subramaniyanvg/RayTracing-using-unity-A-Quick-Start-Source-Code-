using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PureRandom : Sampler {

	public override void generate_samples ()
	{
		for (int p = 0; p < num_sets; p++) {
			for (int q = 0; q < num_samples; q++) {
				samples.Add (new Vector2 (Rand_float (), Rand_float ()));
			}
		}
	}

	float Rand_float()
	{
		return ((float)Random.Range (0, int.MaxValue) * (1.0f / (float)int.MaxValue));
	}
}

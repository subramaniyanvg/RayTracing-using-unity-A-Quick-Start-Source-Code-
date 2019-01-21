using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regular: Sampler
{
	public Regular(int numSamples):base(numSamples)
	{
		generate_samples ();
	}

	public override void generate_samples ()
	{
		int n = (int) Mathf.Sqrt((float)num_samples);

		for (int j = 0; j < num_sets; j++) 
		{
			for (int p = 0; p < n; p++) 
			{
				for (int q = 0; q < n; q++) 
				{
					Vector2 point = new Vector2 ((q + 0.5f) / n, (p + 0.5f) / n);
					samples.Add (point);
				}
			}
		}
	}
}

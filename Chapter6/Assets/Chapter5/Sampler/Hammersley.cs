using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammersley : Sampler {

	public override void generate_samples ()
	{
		for (int p = 0; p < num_sets; p++)		
			for (int j = 0; j < num_samples; j++) {
				Vector2 pv = new Vector2((float) j / (float) num_samples, (float)phi(j));
				samples.Add(pv);
			}	
	}

	double phi(int j) 
	{
		double x = 0.0;
		double f = 0.5; 

		while (j != 0) 
		{
			x += f * (double) (j % 2);
			j /= 2;
			f *= 0.5; 
		}

		return (x);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sampler
{
	System.Random _random = new System.Random ();

	//Sets "num sets" basically used for sampling the surrounding pixels by different sampling techniques 
	public void set_num_sets(int numSets)
	{
		num_sets = numSets;
	}

	//Derived class has to overide and provide implementation for this method
	public virtual void generate_samples()
	{
	}

	//Gets the numSamples
	public int get_num_samples()
	{
		return num_samples;
	}

	//Shuffle x coordinates of samples
	public void shuffle_x_coordinates()
	{
		for (int p = 0; p < num_sets; p++)
		{
			for (int i = 0; i <  num_samples - 1; i++) 
			{
				int target = UnityEngine.Random.Range(0,int.MaxValue) % num_samples + p * num_samples;
				float temp = samples[i + p * num_samples + 1].x;
				samples[i + p * num_samples + 1] = new Vector2(samples[target].x,samples[i + p * num_samples + 1].y);
				samples[target] = new Vector2(temp,samples[target].y);
			}
		}
	}

	//Shuffle y coordinates of samples
	public void shuffle_y_coordinates()
	{
		for (int p = 0; p < num_sets; p++)
		{
			for (int i = 0; i <  num_samples - 1; i++) 
			{
				int target = UnityEngine.Random.Range(0,int.MaxValue) % num_samples + p * num_samples;
				float temp = samples[i + p * num_samples + 1].y;
				samples[i + p * num_samples + 1] = new Vector2 (samples [i + p * num_samples + 1].x, samples [target].y);
				samples[target] = new Vector2(samples[target].x,temp);
			}
		}
	}

	//Shuffle the generic array
	void Shuffle<T>(T[] array)
	{
		int n = array.Length;
		for (int i = 0; i < n; i++)
		{
			int r = i + _random.Next(n - i);
			T t = array[r];
			array[r] = array[i];
			array[i] = t;
		}
	}

	//Sets up the shuffledindices
	void setup_shuffled_indices()
	{
		List<int> indices = new List<int>();

		for (int j = 0; j < num_samples; j++)
			indices.Add(j);

		for (int p = 0; p < num_sets; p++)
		{
			int[] array = indices.ToArray ();
			Shuffle(array);	

			for (int j = 0; j < num_samples; j++)
				shuffled_indices.Add(indices[j]);
		}	
	}

	//map samples to unit disk 
	public void map_samples_to_unit_disk()
	{
		int size = samples.Count;
		float r, phi;		// polar coordinates
		Vector2 sp; 		// sample point on unit disk

		for(int i = 0 ; i < size ; i++)
			disk_samples.Add(Vector2.positiveInfinity);

		for (int j = 0; j < size; j++) 
		{
			// map sample point to [-1, 1] X [-1,1]

			sp.x = 2.0f * samples[j].x - 1.0f;	
			sp.y = 2.0f * samples[j].y - 1.0f;

			if (sp.x > -sp.y) 
			{			// sectors 1 and 2
				if (sp.x > sp.y) 
				{		// sector 1
					r = sp.x;
					phi = sp.y / sp.x;
				}
				else 
				{					// sector 2
					r = sp.y;
					phi = 2 - sp.x / sp.y;
				}
			}
			else 
			{						// sectors 3 and 4
				if (sp.x < sp.y) 
				{		// sector 3
					r = -sp.x;
					phi = 4 + sp.y / sp.x;
				}
				else
				{					// sector 4
					r = -sp.y;
					if (sp.y != 0.0)	// avoid division by zero at origin
						phi = 6.0f - sp.x / sp.y;
					else
						phi  = 0.0f;
				}
			}

			phi *= Mathf.PI / 4.0f;

			disk_samples[j] = new Vector2(r * Mathf.Cos(phi), r * Mathf.Sin(phi));
		}
	}

	//map samples to hemisphere
	public void map_samples_to_hemisphere(float exp)
	{
		int size = samples.Count;
		
		for (int j = 0; j < size; j++) {
			float cos_phi = Mathf.Cos(2.0f * Mathf.PI * samples[j].x);
			float sin_phi = Mathf.Sin(2.0f * Mathf.PI * samples[j].x);	
			float cos_theta = Mathf.Pow((1.0f - samples[j].y), 1.0f / (exp + 1.0f));
			float sin_theta = Mathf.Sqrt (1.0f - cos_theta * cos_theta);
			float pu = sin_theta * cos_phi;
			float pv = sin_theta * sin_phi;
			float pw = cos_theta;
			hemisphere_samples.Add(new Vector3(pu, pv, pw)); 
		}
	}

	//map samples to sphere
	public void map_samples_to_sphere()
	{
		float r1, r2;
		float x, y, z;
		float r, phi;

		for (int j = 0; j < samples.Count; j++) 
		{
			r1 	= samples[j].x;
			r2 	= samples[j].y;
			z 	= 1.0f - 2.0f * r1;
			r 	= Mathf.Sqrt(1.0f - z * z);
			phi = Mathf.PI * 2 * r2;
			x 	= r * Mathf.Cos(phi);
			y 	= r * Mathf.Sin(phi);
			sphere_samples.Add(new Vector3(x, y, z)); 
		}
	}

	public Vector2 sample_unit_square()
	{
		int finalSampleInd = -1;
		while(true)
		{
			if (count % num_samples == 0)  									// start of a new pixel
				jump = (UnityEngine.Random.Range(0,int.MaxValue) % num_sets) * num_samples;				// random index jump initialised to zero in constructor
			count++;
			if (count >= int.MaxValue)
				count = 0;
			int shufffledJumpInd = shuffled_indices[jump + count % num_samples];
			finalSampleInd = jump + shufffledJumpInd;
			if(finalSampleInd > samples.Count - 1)
			{
				count--;
				jump = (UnityEngine.Random.Range(0,int.MaxValue) % num_sets) * num_samples;
				finalSampleInd = -1;
				continue;
			}
			else
				break;
		}
		return samples [finalSampleInd];  
	}

	public Vector2	sample_unit_disk()
	{
		int finalSampleInd = -1;
		while(true)
		{
			if (count % num_samples == 0)  									// start of a new pixel
				jump = (UnityEngine.Random.Range(0,int.MaxValue) % num_sets) * num_samples;				// random index jump initialised to zero in constructor
			count++;
			if (count >= int.MaxValue)
				count = 0;
			int shufffledJumpInd = shuffled_indices[jump + count % num_samples];
			finalSampleInd = jump + shufffledJumpInd;
			if(finalSampleInd > disk_samples.Count - 1)
			{
				count--;
				jump = (UnityEngine.Random.Range(0,int.MaxValue) % num_sets) * num_samples;
				finalSampleInd = -1;
				continue;
			}
			else
				break;
		}
		return disk_samples [finalSampleInd];  
	}

	public Vector3	sample_hemisphere()
	{
		int finalSampleInd = -1;
		while(true)
		{
			if (count % num_samples == 0)  									// start of a new pixel
				jump = (UnityEngine.Random.Range(0,int.MaxValue) % num_sets) * num_samples;				// random index jump initialised to zero in constructor
			count++;
			if (count >= int.MaxValue)
				count = 0;
			int shufffledJumpInd = shuffled_indices[jump + count % num_samples];
			finalSampleInd = jump + shufffledJumpInd;
			if(finalSampleInd > hemisphere_samples.Count - 1)
			{
				count--;
				jump = (UnityEngine.Random.Range(0,int.MaxValue) % num_sets) * num_samples;
				finalSampleInd = -1;
				continue;
			}
			else
				break;
		}
		return hemisphere_samples [finalSampleInd];  
	}

	public Vector3	sample_sphere()
	{
		int finalSampleInd = -1;
		while(true)
		{
			if (count % num_samples == 0)  									// start of a new pixel
				jump = (UnityEngine.Random.Range(0,int.MaxValue) % num_sets) * num_samples;				// random index jump initialised to zero in constructor
			count++;
			if (count >= int.MaxValue)
				count = 0;
			int shufffledJumpInd = shuffled_indices[jump + count % num_samples];
			finalSampleInd = jump + shufffledJumpInd;
			if(finalSampleInd > sphere_samples.Count - 1)
			{
				count--;
				jump = (UnityEngine.Random.Range(0,int.MaxValue) % num_sets) * num_samples;
				finalSampleInd = -1;
				continue;
			}
			else
				break;
		}
		return sphere_samples [finalSampleInd];  
	}

	public Vector2	sample_one_set()
	{
		int finalSampleInd = -1;
		count++;
		if (count >= int.MaxValue)
			count = 0;
		int index = (count % num_samples); 
		while (index > samples.Count - 1)
		{
			count++;
			if (count > int.MaxValue)
				count = 0;
			index = (count % num_samples);
			finalSampleInd = -1;
		}
		finalSampleInd = (count % num_samples);
		return samples[finalSampleInd];
	}

	public void Init()
	{
		count = 0;
		jump = 0;
		setup_shuffled_indices();
	}

	public int 				num_samples;     		// the number of sample points in a set
	public int 				num_sets;				// the number of sample sets
	public List<Vector2>	samples = new List<Vector2>();				// sample points on a unit square
	public List<int>		shuffled_indices = new List<int>();		// shuffled samples array indices
	public List<Vector2>	disk_samples = new List<Vector2>();			// sample points on a unit disk
	public List<Vector3> 	hemisphere_samples = new List<Vector3>();		// sample points on a unit hemisphere
	public List<Vector3> 	sphere_samples = new List<Vector3>();			// sample points on a unit sphere
	public int 				count;					// the current number of sample points used
	public int 				jump;				
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRayPlaneIntersection : MonoBehaviour {

	public enum SamplingTechnique
	{
		JITTERED,
		HAMMERSLEY,
		NROOKS,
		REGULAR,
		PURERANDOM
	}
	public enum SampleType
	{
		SAMPLE_SQUARE,
		SAMPLE_DISK,
		SAMPLE_HEMISPHERE,
		SAMPLE_SPHERE,
		SAMPLE_ONE_SET
	}
	public SampleType		 sampleType = SampleType.SAMPLE_SQUARE;
	public SamplingTechnique samplngTechqueToUse = SamplingTechnique.REGULAR;
	float hemsphereSamplExpRate = 1;
	Vector3 eye = new Vector3 (0, 0, 500);
	Vector3 u = new Vector3(1,0,0);
	Vector3 v = Vector3.up;
	Vector3 w = new Vector3(0,0,1);
	public int 	   numsamples = 20;
	int 	   numsets = 83;

	float   d = 500;

	float epsilon =  0.0001f;
	Vector3 rayDir = Vector3.zero;
	Texture2D texture = null;
	public Vector3 planeNormal = new Vector3 (0, 1, 0);
	public Vector3 planePassThrghPnt = new Vector3 (0, -1, 0);
	Sampler sampler = new Regular();
	Vector2 sp = Vector2.zero;

	//Init sampler 
	void InitSampler()
	{
		if(samplngTechqueToUse == SamplingTechnique.REGULAR)
			sampler = new Regular ();
		else if(samplngTechqueToUse == SamplingTechnique.JITTERED)
			sampler = new Jittered ();
		else if(samplngTechqueToUse == SamplingTechnique.NROOKS)
			sampler = new NRooks ();
		else if(samplngTechqueToUse == SamplingTechnique.HAMMERSLEY)
			sampler = new Hammersley ();
		else if(samplngTechqueToUse == SamplingTechnique.PURERANDOM)
			sampler = new PureRandom ();
		sampler.num_samples = numsamples;
		sampler.num_sets = numsets;
		sampler.Init ();
		sampler.generate_samples ();
		if (sampleType == SampleType.SAMPLE_DISK)
			sampler.map_samples_to_unit_disk ();
		else if (sampleType == SampleType.SAMPLE_HEMISPHERE)
			sampler.map_samples_to_hemisphere (hemsphereSamplExpRate);
		else if (sampleType == SampleType.SAMPLE_SPHERE)
			sampler.map_samples_to_sphere ();

		sp = Vector2.zero;
	}

	//Get Sample for pixel.
	Vector2 GetSampleForPixel()
	{
		if (sampleType == SampleType.SAMPLE_SQUARE)
			sp = sampler.sample_unit_square ();
		else if (sampleType == SampleType.SAMPLE_DISK)
			sp = sampler.sample_unit_disk ();
		else if (sampleType == SampleType.SAMPLE_HEMISPHERE)
			sp = sampler.sample_hemisphere ();
		else if (sampleType == SampleType.SAMPLE_SPHERE)
			sp = sampler.sample_sphere ();
		else if (sampleType == SampleType.SAMPLE_ONE_SET)
			sp = sampler.sample_one_set ();
		return sp;	
	}

	//Here is the core meat where we render the image
	void RenderImage()
	{
		Vector3 rayOrigin = eye;
		//y = 0 means bottom left pixel.
		for (int y = 0; y < texture.height; y++)
		{
			//x = 0 means bottom left pixel.
			for (int x = 0; x < texture.width; x++)
			{
				Color color = Color.black;
				//For the current pixel get the surrounding pixles by iterating for num_samples times. For each iteraction shoot the ray for that pixel and get the  hitcolor if there is one.  
				for (int k = 0; k < sampler.num_samples; k++) 
				{
					sp = GetSampleForPixel ();
					float xPoint = x - ((int)(0.5f * texture.width) + sp.x);
					float yPoint = y - ((int)(0.5f * texture.height) + sp.y);
					rayDir = GetDirection (new Vector2 (xPoint, yPoint));
					color += GetHitColor (rayOrigin);
				}
				//Avergae the color by dividing it by num_samples.
				color /= sampler.num_samples;
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
	}

	// Use this for initialization
	void Start () {
		texture = new Texture2D(200,200);
		GetComponent<Renderer>().material.mainTexture = texture;
		InitSampler ();
		RenderImage ();
	}

	Color GetHitColor(Vector3 rayOrigin)
	{
		Color col = Color.black;
		float t = Vector3.Dot((planePassThrghPnt - rayOrigin),planeNormal) / Vector3.Dot(rayDir,planeNormal);
		if (t >= epsilon) 
		{
			Vector3 point = new Vector3 (rayOrigin.x, rayOrigin.y, rayOrigin.z) + t * rayDir;
			planeNormal = planeNormal;
			col = Color.red;
			return col;
		}
		return col;
	}

	Vector3 GetDirection(Vector2 point)
	{
		Vector3 dir = point.x * u + point.y * v - d * w;
		return dir.normalized;
	}
}

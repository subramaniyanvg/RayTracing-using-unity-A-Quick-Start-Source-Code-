using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : Lighting 
{
	public Sampler 	  sampler_ptr;
	public Matrial 	  materail_ptr;
	public Vector3 	  sample_point;
	public Vector3 	  u,v,w;
	public Vector3	  wi;

	public override Vector3	get_direction(ref Shade s)
	{
		w = s.normal;
		v = Vector3.Cross (new Vector3 (0.0034f, 1.0f, 0.0071f), w);
		v.Normalize ();
		u = Vector3.Cross (v, w);
		Vector3 sp = sampler_ptr.sample_hemisphere ();
		wi = sp.x * u + sp.y * v + sp.z * w;
		return wi;
	}

	public void set_material(Matrial matPtr)
	{
		materail_ptr = matPtr;
	}

	public void SetSampler(Sampler samplr_ptr)
	{
		sampler_ptr = samplr_ptr;
		sampler_ptr.map_samples_to_hemisphere (1);
	}

	public override Color L(ref Shade s)
	{
		return materail_ptr.get_le ();
	}

	public override float G(ref Shade s)
	{
		return 1.0f;
	}

	public override float pdf(ref Shade s)
	{
		return 1.0f;
	}
}
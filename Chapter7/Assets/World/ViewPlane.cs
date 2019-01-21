using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPlane
{
	public int 			hres;   					// horizontal image resolution 
	public int 			vres;   					// vertical image resolution
	public float		s;							// pixel size
	public int			num_samples;				// number of samples per pixel
	public Sampler		sample_ptr;

	public ViewPlane()
	{
		hres = 200;
		vres = 200;
		set_pixel_size (1.0f);
		set_samples (1);
	}

	public ViewPlane(int hres,int vres,float s,int numSamples)
	{
		this.hres = hres;
		this.vres = vres;
		set_pixel_size (s);
		set_samples (numSamples);
	}

	public void set_hres(int h_res)
	{
		hres = h_res;
	}

	public void set_vres(int v_res)
	{
		vres = v_res;
	}

	public void set_pixel_size(float size)
	{
		s = size;
	}

	public void set_sampler(Sampler sp)
	{
		if (sample_ptr != null)
			sample_ptr = null;
		num_samples = sp.get_num_samples();
		sample_ptr =  sp;
	}

	public void set_samples(int n)
	{
		if (sample_ptr != null)
			sample_ptr = null;
		num_samples = n;
		if (num_samples > 1) 
			sample_ptr = new Jittered (num_samples);
		else 
			sample_ptr = new Regular (1);
	}
}
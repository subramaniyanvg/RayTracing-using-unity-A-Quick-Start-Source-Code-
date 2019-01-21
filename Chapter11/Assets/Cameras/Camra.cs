using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camra  
{
	public Vector3			eye;				// eye point
	public Vector3			lookat; 			// lookat point
	public float			ra;					// roll angle
	public Vector3			u, v, w;			// orthonormal basis vectors
	public Vector3			up;					// up vector
	public float			exposure_time;

	public Camra()
	{
		eye  = new Vector3 (0, 0, 500);
		lookat = Vector3.zero;
		up = new Vector3 (0, 1, 0);
		u = new Vector3 (1, 0, 0);
		v = new Vector3 (0, 1, 0);
		w = new Vector3 (0, 0, 1);
		exposure_time = 1.0f;
	}

	public virtual void render_scene(World w)
	{
	}

	public void set_eye(Vector3  p)
	{
		eye = p;
	}

	public void set_lookat(Vector3 p)
	{
		lookat = p;
	}

	public void set_up_vector(Vector3 u)
	{
		up = u;
	}

	public void set_roll(float r)
	{
		ra = r;
	}

	public void set_exposure_time(float exposure)
	{
		exposure_time = exposure;
	}

	public void	compute_uvw()
	{
		w = eye - lookat;
		w.Normalize();
		u = Vector3.Cross(up,w); 
		u.Normalize();
		v = Vector3.Cross(w,u);

		// take care of the singularity by hardwiring in specific camera orientations
		if (eye.x == lookat.x && eye.z == lookat.z && eye.y > lookat.y) { // camera looking vertically down
			u = new Vector3(0, 0, 1);
			v = new Vector3(1, 0, 0);
			w = new Vector3(0, 1, 0);	
		}

		if (eye.x == lookat.x && eye.z == lookat.z && eye.y < lookat.y) { // camera looking vertically up
			u = new Vector3(1, 0, 0);
			v = new Vector3(0, 0, 1);
			w = new Vector3(0, -1, 0);
		}
	}
}
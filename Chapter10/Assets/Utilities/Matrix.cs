using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix 
{
		
	public float[,]	m = new float[4,4];								
	
	public Matrix()
	{
		set_identity ();
	}

	public static Matrix operator* (Matrix matA,Matrix matB)
	{
		Matrix 	product = new Matrix();

		for (int y = 0; y < 4; y++) 
		{
			for (int x = 0; x < 4; x++) 
			{
				float sum = 0.0f;

				for (int j = 0; j < 4; j++)
					sum += matA.m [x,j] * matB.m [j,y];

				product.m [x,y] = sum;			
			}
		}
		return (product);
	}

	public static  Matrix operator/ (Matrix matA,float d)
	{
		for (int x = 0; x < 4; x++) 
		{
			for (int y = 0; y < 4; y++)
				matA.m [x, y] = matA.m [x, y] / d;	
		}
		return matA;
	}
	
    public void	set_identity()
	{
		for (int x = 0; x < 4; x++)
		{
			for (int y = 0; y < 4; y++) 
			{
				if (x == y)
					m[x,y] = 1.0f;
				else
					m [x,y] = 0.0f;
			}
		}
	}

	public static Matrix InverseMatrix(Matrix orig)
	{
		Matrix m  = new Matrix();

		float s0 = orig.m[0,0] * orig.m[1,1] - orig.m[1,0] * orig.m[0,1];
		float s1 = orig.m[0,0] * orig.m[1,2] - orig.m[1,0] * orig.m[0,2];
		float s2 = orig.m[0,0] * orig.m[1,3] - orig.m[1,0] * orig.m[0,3];
		float s3 = orig.m[0,1] * orig.m[1,2] - orig.m[1,1] * orig.m[0,2];
		float s4 = orig.m[0,1] * orig.m[1,3] - orig.m[1,1] * orig.m[0,3];
		float s5 = orig.m[0,2] * orig.m[1,3] - orig.m[1,2] * orig.m[0,3];

		float c5 = orig.m[2,2] * orig.m[3,3] - orig.m[3,2] * orig.m[2,3];
		float c4 = orig.m [2, 1] * orig.m [3, 3] - orig.m [3, 1] * orig.m [2, 3];
		float c3 = orig.m[2,1] * orig.m[3,2] - orig.m[3,1] * orig.m[2,2];
		float c2 = orig.m[2,0] * orig.m[3,3] - orig.m[3,0] * orig.m[2,3];
		float c1 = orig.m[2,0] * orig.m[3,2] - orig.m[3,0] * orig.m[2,2];
		float c0 = orig.m[2,0] * orig.m[3,1] - orig.m[3,0] * orig.m[2,1];

		// Should check for 0 determinant

		float invdet  = 1.0f / (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);

		m.m[0,0] = (orig.m[1,1] * c5 - orig.m[1,2] * c4 + orig.m[1,3] * c3) * invdet;
		m.m[0,1] = (-orig.m[0,1] * c5 + orig.m[0,2] * c4 - orig.m[0,3] * c3) * invdet;
		m.m[0,2] = (orig.m[3,1] * s5 - orig.m[3,2] * s4 + orig.m[3,3] * s3) * invdet;
		m.m[0,3] = (-orig.m[2,1] * s5 + orig.m[2,2] * s4 - orig.m[2,3] * s3) * invdet;

		m.m[1,0] = (-orig.m[1,0] * c5 + orig.m[1,2] * c2 - orig.m[1,3] * c1) * invdet;
		m.m[1,1] = (orig.m[0,0] * c5 - orig.m[0,2] * c2 + orig.m[0,3] * c1) * invdet;
		m.m[1,2] = (-orig.m[3,0] * s5 + orig.m[3,2] * s2 - orig.m[3,3] * s1) * invdet;
		m.m[1,3] = (orig.m[2,0] * s5 - orig.m[2,2] * s2 + orig.m[2,3] * s1) * invdet;

		m.m[2,0] = (orig.m[1,0] * c4 - orig.m[1,1] * c2 + orig.m[1,3] * c0) * invdet;
		m.m[2,1] = (-orig.m[0,0] * c4 + orig.m[0,1] * c2 - orig.m[0,3] * c0) * invdet;
		m.m[2,2] = (orig.m[3,0] * s4 - orig.m[3,1] * s2 + orig.m[3,3] * s0) * invdet;
		m.m[2,3] = (-orig.m[2,0] * s4 + orig.m[2,1] * s2 - orig.m[2,3] * s0) * invdet;

		m.m[3,0] = (-orig.m[1,0] * c3 + orig.m[1,1] * c1 - orig.m[1,2] * c0) * invdet;
		m.m[3,1] = (orig.m[0,0] * c3 - orig.m[0,1] * c1 + orig.m[0,2] * c0) * invdet;
		m.m[3,2] = (-orig.m[3,0] * s3 + orig.m[3,1] * s1 - orig.m[3,2] * s0) * invdet;
		m.m[3,3] = (orig.m[2,0] * s3 - orig.m[2,1] * s1 + orig.m[2,2] * s0) * invdet;

		return m;
	}

	public static Vector3 MultiplyPoint (Matrix mat,Vector3 p)
	{
		return new Vector3((float)(mat.m[0,0] * p.x + mat.m[0,1] * p.y + mat.m[0,2] * p.z + mat.m[0,3]),(float)(mat.m[1,0] * p.x + mat.m[1,1] * p.y + mat.m[1,2] * p.z + mat.m[1,3]),(float)(mat.m[2,0] * p.x + mat.m[2,1] * p.y + mat.m[2,2] * p.z + mat.m[2,3]));
	}

	public static Vector3 MultiplyVector (Matrix mat,Vector3 v,bool canNormalize = false)
	{
		Vector3 result = new Vector3 ((float)(mat.m [0,0] * v.x + mat.m [0,1] * v.y + mat.m [0,2] * v.z), mat.m [1,0] * v.x + mat.m [1,1] * v.y + mat.m [1,2] * v.z, mat.m [2,0] * v.x + mat.m [2,1] * v.y + mat.m [2,2] * v.z);
		if(canNormalize)
			return result.normalized;
		return result;
	}
}
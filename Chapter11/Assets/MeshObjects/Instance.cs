using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instance: MeshObject
{
	public MeshObject 		  obj_ptr;
	public Matrial			  mat_Ptr;
	public Matrix 			  inv_matrix;

	public Instance()
	{
		inv_matrix = new Matrix ();
	}

	public Instance(MeshObject obj)
	{
		inv_matrix = new Matrix ();
		obj_ptr = obj;
	}

	public override bool hit(ref Ray ray,ref float t,ref Shade s)
	{
		Ray inv_ray = ray;
		inv_ray.origin = Matrix.MultiplyPoint (inv_matrix, inv_ray.origin);
		inv_ray.direction = Matrix.MultiplyVector (inv_matrix, inv_ray.direction);
		if (obj_ptr.hit (ref inv_ray, ref t, ref s)) 
		{
			s.normal = Matrix.MultiplyVector (inv_matrix, s.normal, true);
			if (obj_ptr.get_material () != null)
				mat_Ptr = obj_ptr.get_material ();
			return true;
		}
		return false;
	}

	public override bool shadow_hit(ref Ray ray,ref float t)
	{
		return false;
	}

	public void set_identity()
	{
		inv_matrix.set_identity ();
	}

	public void Translate(float x,float y,float z)
	{
		Matrix inv_trans_mat = new Matrix ();
		inv_trans_mat.m [0,3] = -x;
		inv_trans_mat.m [1,3] = -y;
		inv_trans_mat.m [2,3] = -z;

		inv_matrix = inv_matrix * inv_trans_mat;
	}

	public void ReflectInX()
	{
		Matrix inv_trans_mat = new Matrix ();
		inv_trans_mat.m [0,0] = -1;
		inv_matrix = inv_matrix * inv_trans_mat;
	}

	public void ReflectInY()
	{
		Matrix inv_trans_mat = new Matrix ();
		inv_trans_mat.m [1,1] = -1;
		inv_matrix = inv_matrix * inv_trans_mat;
	}

	public void ReflectInZ()
	{
		Matrix inv_trans_mat = new Matrix ();
		inv_trans_mat.m [2,2] = -1;
		inv_matrix = inv_matrix * inv_trans_mat;
	}

	public void Shear(float hyx,float hzx,float hxy,float hzy,float hxz,float hyz)
	{
		Matrix inv_trans_mat = new Matrix ();
		Matrix shear = new Matrix ();

		shear.m [0, 1] = hyx;
		shear.m [0, 2] = hzx;

		shear.m [1, 0] = hxy;
		shear.m [1, 2] = hzy;

		shear.m [2, 0] = hxz;
		shear.m [2, 1] = hyz;

		inv_trans_mat = Matrix.InverseMatrix (shear);

		inv_matrix = inv_matrix * inv_trans_mat;
	}

	public void Scale(float x,float y,float z)
	{
		Matrix inv_trans_mat = new Matrix ();
		inv_trans_mat.m [0,0] = 1/x;
		inv_trans_mat.m [1,1] = 1/y;
		inv_trans_mat.m [2,2] = 1/z;

		inv_matrix = inv_matrix * inv_trans_mat;
	}

	public void Rotate(float xRadians,float yRadians, float zRadians)
	{
		Matrix inv_trans_mat = new Matrix ();

		inv_trans_mat.m [1,1] = Mathf.Cos(xRadians);
		inv_trans_mat.m [1,2] = Mathf.Sin(xRadians);

		inv_trans_mat.m [2, 1] = -Mathf.Sin (xRadians);
		inv_trans_mat.m [2,2] = Mathf.Cos(xRadians);


		inv_matrix = inv_matrix * inv_trans_mat;

		inv_trans_mat.set_identity ();

		inv_trans_mat.m [0,0] = Mathf.Cos(yRadians);
		inv_trans_mat.m [0,2] = -Mathf.Sin(yRadians);

		inv_trans_mat.m [2, 0] = Mathf.Sin (yRadians);
		inv_trans_mat.m [2,2] = Mathf.Cos(yRadians);


		inv_matrix = inv_matrix * inv_trans_mat;

		inv_trans_mat.set_identity ();

		inv_trans_mat.m [0,0] = Mathf.Cos(zRadians);
		inv_trans_mat.m [0,1] = Mathf.Sin(zRadians);

		inv_trans_mat.m [1,0] = -Mathf.Sin (zRadians);
		inv_trans_mat.m [1,1] = Mathf.Cos(zRadians);


		inv_matrix = inv_matrix * inv_trans_mat;

	}

	public void RotateAroundLine(Vector3 instPos,Vector3 lineDir,float rotAmt)
	{
		lineDir.Normalize ();
		Vector3 diff  = Vector3.zero - instPos;

		Matrix transMat = new Matrix ();
		transMat.m[0,3] = diff.x;
		transMat.m [1,3] = diff.y;
		transMat.m [2,3] = diff.z;


		Vector3 v = Vector3.Cross(Vector3.up,lineDir);
		v.Normalize ();
		Vector3 u = Vector3.Cross(lineDir,v);
		u.Normalize ();

		Matrix rTanspose = new Matrix ();

		rTanspose.m [0,0] = u.x;
		rTanspose.m [1,0] = u.y;
		rTanspose.m [2,0] = u.z;

		rTanspose.m [0,1] = v.x;
		rTanspose.m [1,1] = v.y;
		rTanspose.m [2,1] = v.z;

		rTanspose.m [0,2] = lineDir.x;
		rTanspose.m [1,2] = lineDir.y;
		rTanspose.m [2,2] = lineDir.z;


		Matrix rotZMat = new Matrix ();

		rotZMat.m [0,0] = Mathf.Cos (rotAmt);
		rotZMat.m [1,0] = -Mathf.Sin (rotAmt);
				  
		rotZMat.m [0,1] = Mathf.Sin (rotAmt);
		rotZMat.m [1,1] = Mathf.Cos (rotAmt);


		Matrix r = new Matrix ();

		r.m[0,0] = u.x;
		r.m[1,0] = v.x;
		r.m[2,0] = lineDir.x;
		   
		r.m[0,1] = u.y;
		r.m[1,1] = v.y;
		r.m[2,1] = lineDir.y;
		   
		r.m[0,2] = u.z;
		r.m[1,2] = v.z;
		r.m[2,2] = lineDir.z;

		Matrix invtransMat = new Matrix ();
		transMat.m [0,3] = -diff.x;
		transMat.m [1,3] = -diff.y;
		transMat.m [2,3] = -diff.z;

		inv_matrix = inv_matrix * transMat * rTanspose * rotZMat * r * invtransMat;
	}

	public void RotateAroundLine(Vector3 instPos,Vector3 lineDir,Vector3 lineRightDir,Vector3 lineUpDir,float rotAmt)
	{
		lineDir.Normalize ();
		lineRightDir.Normalize ();
		lineUpDir.Normalize ();
		Vector3 diff  = Vector3.zero - instPos;

		Matrix transMat = new Matrix ();
		transMat.m[0,3] = diff.x;
		transMat.m [1,3] = diff.y;
		transMat.m [2,3] = diff.z;

		Matrix rTanspose = new Matrix ();

		rTanspose.m [0,0] = lineUpDir.x;
		rTanspose.m [1,0] = lineUpDir.y;
		rTanspose.m [2,0] = lineUpDir.z;

		rTanspose.m [0,1] = lineRightDir.x;
		rTanspose.m [1,1] = lineRightDir.y;
		rTanspose.m [2,1] = lineRightDir.z;

		rTanspose.m [0,2] = lineDir.x;
		rTanspose.m [1,2] = lineDir.y;
		rTanspose.m [2,2] = lineDir.z;


		Matrix rotZMat = new Matrix ();

		rotZMat.m [0,0] = Mathf.Cos (rotAmt);
		rotZMat.m [1,0] = -Mathf.Sin (rotAmt);

		rotZMat.m [0,1] = Mathf.Sin (rotAmt);
		rotZMat.m [1,1] = Mathf.Cos (rotAmt);


		Matrix r = new Matrix ();

		r.m[0,0] = lineUpDir.x;
		r.m[1,0] = lineRightDir.x;
		r.m[2,0] = lineDir.x;

		r.m[0,1] = lineUpDir.y;
		r.m[1,1] = lineRightDir.y;
		r.m[2,1] = lineDir.y;

		r.m[0,2] = lineUpDir.z;
		r.m[1,2] = lineRightDir.z;
		r.m[2,2] = lineDir.z;

		Matrix invtransMat = new Matrix ();
		transMat.m [0,3] = -diff.x;
		transMat.m [1,3] = -diff.y;
		transMat.m [2,3] = -diff.z;

		inv_matrix = inv_matrix * transMat * rTanspose * rotZMat * r * invtransMat;
	}
}
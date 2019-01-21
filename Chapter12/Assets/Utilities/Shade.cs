using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shade 
{
	public MeshObject 		hitMesh;
	public bool				hit_an_object = false;		
	public Matrial 			material_ptr = null;		
	public Vector3 			hit_point = Vector3.positiveInfinity;			
	public Vector3			local_hit_point = Vector3.positiveInfinity;	
	public Vector3			normal = Vector3.positiveInfinity;				
	public Ray				ray = new Ray(Vector3.zero,Vector3.zero);				
	public int				depth = int.MaxValue;				
	public float			t = Mathf.Infinity;	
	public World			w = null;
	public Color          	color = Constants.black;
}
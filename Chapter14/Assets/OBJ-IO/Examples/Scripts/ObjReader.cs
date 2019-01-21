
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityExtension;

using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter))]
public class ObjReader : MonoBehaviour
{
	//------------------------------------------------------------------------------------------------------------	
	private const string INPUT_PATH = @"Assets/OBJ-IO/Examples/Meshes/Quad.obj";
	private const string OUTPUT_PATH = @"Assets/OBJ-IO/Examples/Meshes/Teapot_Modified.obj";

    //------------------------------------------------------------------------------------------------------------	
	private void Start()
	{
		//	Load the OBJ in
		var lStream = new FileStream(INPUT_PATH, FileMode.Open);
		var lOBJData = OBJLoader.LoadOBJ(lStream);
		var lMeshFilter = GetComponent<MeshFilter>();
		lMeshFilter.mesh.LoadOBJ(lOBJData);
		lStream.Close();
		
		lStream = null;
		lOBJData = null;

		//	Wiggle Vertices in Mesh
		List<Vector2> uvs = new List<Vector2>();
		lMeshFilter.mesh.GetUVs (0, uvs);
		var tris = lMeshFilter.mesh.vertices;

		for (int lCount = 0; lCount < tris.Length; lCount ++) 
		{
			Debug.Log("Vert" + tris[lCount]);
			Debug.Log ("Uv" + lMeshFilter.mesh.uv [lCount]);	
		}
//		for (int lCount = 0; lCount < tris.Length; lCount += 3)
//		{
//			Debug.Log (tris [lMeshFilter.mesh.vertices[lCount]]);
//			Debug.Log (tris [lMeshFilter.mesh.vertices[lCount + 2]]);
//			Debug.Log (tris [lMeshFilter.mesh.vertices[lCount +  1]]);
//
//			Debug.Log (tris [lMeshFilter.mesh.uv[lCount]]);
//			Debug.Log (tris [lMeshFilter.mesh.uv[lCount + 2]]);
//			Debug.Log (tris [lMeshFilter.mesh.uv[lCount +  1]]);
//
//		}
		//lMeshFilter.mesh.vertices = lVertices;
		
	}
}
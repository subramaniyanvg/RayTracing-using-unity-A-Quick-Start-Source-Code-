using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExtension;


public class Model : MonoBehaviour 
{
	
	[System.Serializable]
	public class MeshDataProp
	{
		public Vector3 vertex = Vector3.positiveInfinity;
		public Vector2 uv = Vector2.positiveInfinity;
	}

	List<Instance> instanceList = new List<Instance>();

	public Model(string modelName,string textureName,int instanceId, out TextureData textData)
	{
		World worpldPtr = Camera.main.gameObject.GetComponent<World> ();
		//	Load the OBJ in
		var lStream = new FileStream(Application.dataPath + "/Resources/" + modelName + ".obj", FileMode.Open);
		var lOBJData = OBJLoader.LoadOBJ(lStream);
		UnityEngine.Object obj = Resources.Load (modelName);
		GameObject gameObj = (GameObject)Instantiate(obj);

		Texture2D mapTexture = Resources.Load<Texture2D> (textureName);
		TextureData data = new TextureData ();
		data.LoadTextureData (mapTexture);

		textData = data;

		var lMeshFilter = gameObj.GetComponentInChildren<MeshFilter>();
		lMeshFilter.gameObject.GetComponent<MeshRenderer> ().enabled = false;
		lMeshFilter.mesh.LoadOBJ(lOBJData);
		lStream.Close();

		lStream = null;
		lOBJData = null;


		var verts = lMeshFilter.mesh.vertices;
		//	Wiggle Vertices in Mesh
		List<Vector2> uvs = new List<Vector2>();
		lMeshFilter.mesh.GetUVs (0, uvs);

		List<Vector3> normals = new List<Vector3>();
		lMeshFilter.mesh.GetNormals (normals);

		List<MeshDataProp> meshDataList = new List<MeshDataProp>();

		for(int i = 0 ; i < verts.Length ; i++)
		{
			MeshDataProp meshData = new MeshDataProp ();
			meshData.vertex = verts [i];
			meshData.uv = uvs [i]; 
			meshDataList.Add (meshData);
		}

		var tris = lMeshFilter.mesh.triangles;
		for (int lCount = 0; lCount < tris.Length; lCount += 3) 
		{
			Triangle triangle = new Triangle (meshDataList[tris[lCount]].vertex,meshDataList[tris[lCount + 1]].vertex,meshDataList[tris[lCount + 2]].vertex,meshDataList[tris[lCount]].uv,meshDataList[tris[lCount + 1]].uv,meshDataList[tris[lCount + 2]].uv,true);
			Instance triangleInst = new Instance(triangle);
            triangleInst.instanceId = instanceId;
            instanceList.Add (triangleInst);
			worpldPtr.add_object (triangleInst);
		}
	}

	public void SetMaterial(Matrial mat)
	{
		for(int i = 0 ; i < instanceList.Count ; i++)
			instanceList [i].set_material (mat);
	}

	public void set_identity()
	{
		for(int i = 0 ; i < instanceList.Count ; i++)
			instanceList [i].set_identity();
	}

	public void Translate(float x,float y,float z)
	{
		for(int i = 0 ; i < instanceList.Count ; i++)
			instanceList [i].Translate(x,y,z);
	}

	public void ReflectInX()
	{
		for(int i = 0 ; i < instanceList.Count ; i++)
			instanceList [i].ReflectInX();
	}

	public void ReflectInY()
	{
		for(int i = 0 ; i < instanceList.Count ; i++)
			instanceList [i].ReflectInY();
	}

	public void ReflectInZ()
	{
		for(int i = 0 ; i < instanceList.Count ; i++)
			instanceList [i].ReflectInZ();
	}

	public void Shear(float hyx,float hzx,float hxy,float hzy,float hxz,float hyz)
	{
		for(int i = 0 ; i < instanceList.Count ; i++)
			instanceList [i].Shear(hyx,hzx,hxy,hzy,hxz,hyz);
	}

	public void Scale(float x,float y,float z)
	{
		for(int i = 0 ; i < instanceList.Count ; i++)
			instanceList [i].Scale(x,y,z);
	}

	public void Rotate(float xRadians,float yRadians, float zRadians)
	{
		for(int i = 0 ; i < instanceList.Count ; i++)
			instanceList [i].Rotate(xRadians,yRadians,zRadians);
	}

	public void RotateAroundLine(Vector3 instPos,Vector3 lineDir,float rotAmt)
	{
		for(int i = 0 ; i < instanceList.Count ; i++)
			instanceList [i].RotateAroundLine(instPos,lineDir,rotAmt);
	}

	public void RotateAroundLine(Vector3 instPos,Vector3 lineDir,Vector3 lineRightDir,Vector3 lineUpDir,float rotAmt)
	{
		for(int i = 0 ; i < instanceList.Count ; i++)
			instanceList [i].RotateAroundLine(instPos,lineDir,lineRightDir,lineUpDir,rotAmt);
	}
}
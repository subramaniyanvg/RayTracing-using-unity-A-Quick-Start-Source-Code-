using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityExtension;

public class World : MonoBehaviour 
{
	[HideInInspector]
	public Texture2D			texture;
	[HideInInspector]
	public ViewPlane			vp;
	public Color				background_color;
	[HideInInspector]
	public Tracer				tracer_ptr;
	[HideInInspector]
	public Lighting   			ambient_ptr;
	[HideInInspector]
	public Camra				camera_ptr;		
	[HideInInspector]
	public List<MeshObject>			objects = new List<MeshObject>();		
	[HideInInspector]
	public List<Lighting> 		lights = new List<Lighting>();

	[System.Serializable]
	public class MeshDataProp
	{
		public Vector3 vertex = Vector3.positiveInfinity;
		public Vector3 normal = Vector3.positiveInfinity;
		public Vector2 uv = Vector2.positiveInfinity;
	}
	[HideInInspector]
	public List<MeshDataProp>  meshDataList = new List<MeshDataProp>();

	void Start()
	{
		build ();
	}

	void Update()
	{

	}

	public void DestroyRenderAreaTexture()
	{
		if (texture) 
		{
			Destroy (texture);
			texture = null;
		}
	}

	public void add_object(MeshObject object_ptr)
	{
		objects.Add(object_ptr);	
	}

	public void  add_light(Lighting light_ptr)
	{
		lights.Add(light_ptr);
	}

	public void set_ambient_light(Lighting light_ptr)
	{
		ambient_ptr = light_ptr;
	}

	public void set_camera(Camra c_ptr)
	{
		camera_ptr = c_ptr;
	}

    void MatteRender()
    {
        DestroyRenderAreaTexture();

        texture = new Texture2D(200, 200);
        GameObject.Find("ViewRectangle").GetComponent<MeshRenderer>().material.mainTexture = texture;
        vp = new ViewPlane(texture.width, texture.height, 1.0f, 1);
        vp.max_depth = 1;
        background_color = Constants.white;


        Ambient ambLight = new Ambient();
        ambLight.set_color(new Color(1, 1, 1, 1));
        ambLight.scale_radiance(1.0f);
        set_ambient_light(ambLight);


        PerspectiveCamera pinhole_ptr1 = new PerspectiveCamera();
        pinhole_ptr1.set_eye(new Vector3(0, 0, 500));
        pinhole_ptr1.set_lookat(Vector3.zero);
        pinhole_ptr1.set_view_distance(500.0f);
        pinhole_ptr1.compute_uvw();
        set_camera(pinhole_ptr1);

        Directional directional = new Directional();
        directional.set_color(new Color(1, 1, 1, 1));
        directional.set_direction(new Vector3(0, -1, 0));
        directional.cast_shadows = false;
        directional.scale_radiance(3.0f);
        add_light(directional);


        tracer_ptr = new RayCastTracer(this);

        TextureData textData = new TextureData();

        Model ground = new Model("Quad/Quad", "Quad/Quad_Tex", 1, out textData);

        SV_Matte mat_ptr = new SV_Matte();
        mat_ptr.set_ka(0.25f);
        mat_ptr.set_kd(0.5f);
        mat_ptr.set_cd(textData);

        ground.SetMaterial(mat_ptr);
        ground.set_identity();
        ground.Rotate(Constants.PI / 2, 0, 0);
        ground.Scale(600, 1, 1000);
        ground.Translate(0, -100, 0);


        textData = new TextureData();

		Model rightSphere = new Model("Sphere/Sphere", "Sphere/Sphere_Ball_Tex", 2, out textData);

        mat_ptr = new SV_Matte();
        mat_ptr.set_ka(0.25f);
        mat_ptr.set_kd(0.5f);
        mat_ptr.set_cd(textData);

        rightSphere.SetMaterial(mat_ptr);
        rightSphere.set_identity();
        rightSphere.Scale(40, 40, 40);
        rightSphere.Translate(50.0f, -60, 80);

		Model leftSphere = new Model("Sphere/Sphere", "Sphere/Sphere_Ball_Tex", 2, out textData);

        mat_ptr = new SV_Matte();
        mat_ptr.set_ka(0.25f);
        mat_ptr.set_kd(0.5f);
        mat_ptr.set_cd(textData);

        leftSphere.SetMaterial(mat_ptr);
        leftSphere.set_identity();
        leftSphere.Scale(40, 40, 40);
        leftSphere.Translate(-50.0f, -60, 80);

        Model cube = new Model("Cube/Cube", "Cube/Cube_Tex", 2, out textData);

        mat_ptr = new SV_Matte();
        mat_ptr.set_ka(0.25f);
        mat_ptr.set_kd(0.5f);
        mat_ptr.set_cd(textData);

        cube.SetMaterial(mat_ptr);
        cube.set_identity();
        cube.Scale(40, 100, 40);
        cube.Translate(0.0f, -40, -200);

        Model pyramid = new Model("Pyramid/Pyramid", "Pyramid/Pyramid_Tex", 2, out textData);

        mat_ptr = new SV_Matte();
        mat_ptr.set_ka(0.25f);
        mat_ptr.set_kd(0.5f);
        mat_ptr.set_cd(textData);

        pyramid.SetMaterial(mat_ptr);
        pyramid.set_identity();
        pyramid.Scale(80, 100, 40);
        pyramid.Translate(-100.0f, -50, -800);

        Model pyramidRight = new Model("Pyramid/Pyramid", "Pyramid/Pyramid_Tex", 2, out textData);

        mat_ptr = new SV_Matte();
        mat_ptr.set_ka(0.25f);
        mat_ptr.set_kd(0.5f);
        mat_ptr.set_cd(textData);

        pyramidRight.SetMaterial(mat_ptr);
        pyramidRight.set_identity();
        pyramidRight.Scale(80, 100, 40);
        pyramidRight.Translate(120.0f, -50, -800);


        Model cubeLeft = new Model("Cube/Cube", "Cube/Cube_Tex", 2, out textData);

        mat_ptr = new SV_Matte();
        mat_ptr.set_ka(0.25f);
        mat_ptr.set_kd(0.5f);
        mat_ptr.set_cd(textData);

        cubeLeft.SetMaterial(mat_ptr);
        cubeLeft.set_identity();
        cubeLeft.Scale(40, 100, 40);
        cubeLeft.Translate(-100.0f, -40, -100);


        Model cubeRight = new Model("Cube/Cube", "Cube/Cube_Tex", 2, out textData);

        mat_ptr = new SV_Matte();
        mat_ptr.set_ka(0.25f);
        mat_ptr.set_kd(0.5f);
        mat_ptr.set_cd(textData);

        cubeRight.SetMaterial(mat_ptr);
        cubeRight.set_identity();
        cubeRight.Scale(40, 100, 40);
        cubeRight.Translate(100.0f, -40, -100);
    }

    void ReflectiveRender()
    {
        DestroyRenderAreaTexture();

        texture = new Texture2D(200, 200);
        GameObject.Find("ViewRectangle").GetComponent<MeshRenderer>().material.mainTexture = texture;
        vp = new ViewPlane(texture.width, texture.height, 1.0f, 1);
        vp.max_depth = 5;
        background_color = Constants.white;


        Ambient ambLight = new Ambient();
        ambLight.set_color(new Color(1, 1, 1, 1));
        ambLight.scale_radiance(1.0f);
        set_ambient_light(ambLight);


        PerspectiveCamera pinhole_ptr1 = new PerspectiveCamera();
        pinhole_ptr1.set_eye(new Vector3(0, 0, 500));
        pinhole_ptr1.set_lookat(Vector3.zero);
        pinhole_ptr1.set_view_distance(500.0f);
        pinhole_ptr1.compute_uvw();
        set_camera(pinhole_ptr1);

        Directional directional = new Directional();
        directional.set_color(new Color(1, 1, 1, 1));
        directional.set_direction(new Vector3(0, -1, 0));
        directional.cast_shadows = false;
        directional.scale_radiance(3.0f);
        add_light(directional);


        tracer_ptr = new Whitted(this);

        TextureData textData = new TextureData();

        Model ground = new Model("Quad/Quad", "Quad/Quad_Tex_Col", 1, out textData);

        SV_Reflective ref_ptr = new SV_Reflective();
        ref_ptr.set_kr(1.0f);
        ref_ptr.set_cr(textData);


        ground.SetMaterial(ref_ptr);
        ground.set_identity();
        ground.Rotate(Constants.PI / 2, 0, 0);
        ground.Scale(600, 1, 1000);
        ground.Translate(0, -100, 0);

        textData = new TextureData();

        Model groundup = new Model("Quad/Quad", "Quad/Quad_Tex_Col", 1, out textData);

        ref_ptr = new SV_Reflective();
        ref_ptr.set_kr(1.0f);
        ref_ptr.set_cr(textData);

        groundup.SetMaterial(ref_ptr);
        groundup.set_identity();
        groundup.Rotate(-Constants.PI / 2, 0, 0);
        groundup.Scale(600, 1, 1000);
        groundup.Translate(0, 100, 0);

        textData = new TextureData();

        Model rightSphere = new Model("Sphere/Sphere", "Sphere/Sphere_Tex", 2, out textData);

        ref_ptr = new SV_Reflective();
        ref_ptr.set_kr(1.0f);
        ref_ptr.set_cr(textData);

        rightSphere.SetMaterial(ref_ptr);
        rightSphere.set_identity();
        rightSphere.Scale(40, 40, 40);
        rightSphere.Translate(50.0f, -60, 80);

        textData = new TextureData();

        Model leftSphere = new Model("Sphere/Sphere", "Sphere/Sphere_1_Tex", 2, out textData);

        ref_ptr = new SV_Reflective();
        ref_ptr.set_kr(1.0f);
        ref_ptr.set_cr(textData);

        leftSphere.SetMaterial(ref_ptr);
        leftSphere.set_identity();
        leftSphere.Scale(40, 40, 40);
        leftSphere.Translate(-50.0f, -60, 80);

        textData = new TextureData();
        Model cube = new Model("Cube/Cube", "Cube/Cube_Tex", 2, out textData);

        ref_ptr = new SV_Reflective();
        ref_ptr.set_kr(1.0f);
        ref_ptr.set_cr(textData);

        cube.SetMaterial(ref_ptr);
        cube.set_identity();
        cube.Scale(40, 100, 40);
        cube.Translate(0.0f, -40, -200);

        textData = new TextureData();
        Model pyramid = new Model("Pyramid/Pyramid", "Pyramid/Pyramid_Tex", 2, out textData);

        ref_ptr = new SV_Reflective();
        ref_ptr.set_kr(1.0f);
        ref_ptr.set_cr(textData);

        pyramid.SetMaterial(ref_ptr);
        pyramid.set_identity();
        pyramid.Scale(80, 100, 40);
        pyramid.Translate(-100.0f, -50, -800);

        textData = new TextureData();
        Model pyramidRight = new Model("Pyramid/Pyramid", "Pyramid/Pyramid_Tex", 2, out textData);

        ref_ptr = new SV_Reflective();
        ref_ptr.set_kr(1.0f);
        ref_ptr.set_cr(textData);

        pyramidRight.SetMaterial(ref_ptr);
        pyramidRight.set_identity();
        pyramidRight.Scale(80, 100, 40);
        pyramidRight.Translate(120.0f, -50, -800);


        textData = new TextureData();
        Model cubeLeft = new Model("Cube/Cube", "Cube/Cube_Tex", 2, out textData);

        ref_ptr = new SV_Reflective();
        ref_ptr.set_kr(1.0f);
        ref_ptr.set_cr(textData);

        cubeLeft.SetMaterial(ref_ptr);
        cubeLeft.set_identity();
        cubeLeft.Scale(40, 100, 40);
        cubeLeft.Translate(-100.0f, -40, -100);

        textData = new TextureData();
        Model cubeRight = new Model("Cube/Cube", "Cube/Cube_Tex", 2, out textData);

        ref_ptr = new SV_Reflective();
        ref_ptr.set_kr(1.0f);
        ref_ptr.set_cr(textData);

        cubeRight.SetMaterial(ref_ptr);
        cubeRight.set_identity();
        cubeRight.Scale(40, 100, 40);
        cubeRight.Translate(100.0f, -40, -100);
    }

    public void build()
	{
		ReflectiveRender();
        
        render_scene();
	}

	public void render_scene()
	{
		camera_ptr.render_scene (this);
	}

	public Color max_to_one(Color c)
	{
		Color col = new Color(0,0,0,1);
		float max_value = Mathf.Max(c.r, Mathf.Max(c.g, c.b));
		if (max_value > 1.0)
			col = c / max_value;
		else
			col = c;
		return col;
	}

	public Color clamp_to_color(Color raw_color)
	{
		Color c = raw_color;

		if (raw_color.r > 1.0f || raw_color.g > 1.0f || raw_color.b > 1.0f) 
		{
			c.r = 1.0f; c.g = 0.0f; c.b = 0.0f;
		}

		return (c);
	}

	public void display_pixel(int row,int column,Color pixel_color)
	{
		texture.SetPixel(column,row,pixel_color);
	}

	public Shade hit_objects(Ray ray)
	{
		Shade	shade = new Shade();
		shade.w = this;
		float	t = Mathf.Infinity;
		Vector3 normal =Vector3.positiveInfinity;
		Vector3 local_hit_point = Vector3.positiveInfinity;
		float		tmin 			= Mathf.Infinity;
		int 		num_objects 	= objects.Count;

		for (int j = 0; j < num_objects; j++)
		{
			if (objects [j].hit (ref ray,ref t,ref shade)) 
			{
				if (t == Mathf.Infinity && tmin == Mathf.Infinity) 
				{
					shade.hit_an_object	= true;
					tmin = t;
					shade.material_ptr = objects [j].get_material ();
					shade.hit_point = ray.origin + t * ray.direction;
					normal = shade.normal;
					local_hit_point = shade.local_hit_point;
				} 
				else if (t < tmin) 
				{
					shade.hit_an_object	= true;
					tmin = t;
					shade.material_ptr = objects [j].get_material ();
					shade.hit_point = ray.origin + t * ray.direction;
					normal = shade.normal;
					local_hit_point = shade.local_hit_point;
				}
			}
		}
		if (shade.hit_an_object)
		{
			shade.t = tmin;
			shade.normal = normal;
			shade.local_hit_point = local_hit_point;
		} 
		return(shade);   
	}
}
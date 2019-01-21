using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour 
{
	public Color sphere_1_col;
	public Color plane_col;
	public Color sphere_2_col;


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

	void Start()
	{
		build ();
	}

	void Update()
	{
		//if (Input.GetKeyUp (KeyCode.UpArrow)) 
		{

		}
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

	public void build()
	{
		DestroyRenderAreaTexture ();

		texture = new Texture2D (200, 200);
		GameObject.Find ("ViewRectangle").GetComponent<MeshRenderer> ().material.mainTexture = texture;
		vp = new ViewPlane (texture.width,texture.height,1.0f,1);
		vp.max_depth = 5;

		tracer_ptr = new GlobalTracer (this);

		Ambient amblight = new Ambient();
		amblight.scale_radiance (1.0f);
		amblight.set_color (new Color(1,1,1,1));
		set_ambient_light (amblight);


		PerspectiveCamera pinhole_ptr1 = new PerspectiveCamera();
		pinhole_ptr1.set_eye(new Vector3(0, 0, 500)); 
		pinhole_ptr1.set_lookat(Vector3.zero);
		pinhole_ptr1.set_view_distance(600.0f);
		pinhole_ptr1.compute_uvw();     
		set_camera(pinhole_ptr1);

		Directional directional = new Directional ();
		directional.set_color(new Color(1,1,1,1));
		directional.set_direction(new Vector3(-1,-1,0));
		directional.cast_shadows = false;
		directional.scale_radiance (3.0f);
		add_light (directional);

		Reflective mat_ptr = new Reflective ();
		mat_ptr.set_kr (1.0f);
		mat_ptr.set_exp (1.0f);
		mat_ptr.set_cr (sphere_1_col);

		Sphere sphere = new Sphere ();
		sphere.sphereCenter = new Vector3(0,0,0);
		sphere.sphereRad = 1.0f;
		sphere.set_material (mat_ptr);

		Instance sphereInst = new Instance(sphere);
		sphereInst.set_material (mat_ptr);
		add_object (sphereInst);

		sphereInst.set_identity ();
		sphereInst.Scale (20, 20, 20);
		sphereInst.Translate (60.0f,20.0f,0);

		Matte mat_ptr1 = new Matte ();
		mat_ptr1.ambient_brdf.Set_Sampler (100, 0.5f);
		mat_ptr1.diffuse_brdf.Set_Sampler (100, 0.5f);
		mat_ptr1.set_ka (1.0f);
		mat_ptr1.set_kd (1);
		mat_ptr1.set_cd (plane_col);


		Plane p = new Plane ();
		p.planeNormal = new Vector3 (0, 1, 0);
		p.planePassThrghPnt = new Vector3(0,-20,0);
		p.set_material (mat_ptr1);
		add_object (p);

		Emissive mat_ptr2 = new Emissive ();
		mat_ptr2.scale_radiance (1);
		mat_ptr2.set_ce (sphere_2_col);

		Sphere sphere1 = new Sphere ();
		sphere1.sphereCenter = new Vector3(0,0,0);
		sphere1.sphereRad = 1.0f;
		sphere1.set_material (mat_ptr2);

		Instance sphereInst1 = new Instance(sphere1);
		sphereInst1.set_material (mat_ptr2);
		add_object (sphereInst1);

		sphereInst1.set_identity ();
		sphereInst1.Scale (20, 20, 20);
		sphereInst1.Translate (-60.0f,20.0f,0);

		render_scene ();
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
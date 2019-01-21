using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		background_color = Constants.white;

		tracer_ptr = new Whitted (this);

		Jittered jit = new Jittered(1);
		AmbientOccluder ambocl = new AmbientOccluder ();
		ambocl.scale_radiance (1.0f);
		ambocl.set_color (Constants.white);
		ambocl.set_minAmount (Constants.black);
		ambocl.SetSampler (jit);
		set_ambient_light (ambocl);


		PerspectiveCamera pinhole_ptr1 = new PerspectiveCamera();
		pinhole_ptr1.set_eye(new Vector3(0, 0, 500)); 
		pinhole_ptr1.set_lookat(Vector3.zero);
		pinhole_ptr1.set_view_distance(600.0f);
		pinhole_ptr1.compute_uvw();     
		set_camera(pinhole_ptr1);

		Directional directional = new Directional ();
		directional.set_color(new Color(1,1,1,1));
		directional.set_direction(new Vector3(-1,-1,0));
		directional.cast_shadows = true;
		directional.scale_radiance (3.0f);
		add_light (directional);

		Dielectric mat_ptr = new Dielectric ();
		mat_ptr.set_eta_in (0.8f);
		mat_ptr.set_eta_in (1.2f);
		mat_ptr.set_cf_in (new Color (0.0f, 1.0f, 0.8f, 1));
		mat_ptr.set_cf_out (new Color (0.75f, 1.0f, 0.4f, 1));
		mat_ptr.set_ks (0.5f);
		mat_ptr.set_exp (100.0f);
		mat_ptr.set_ior (1.2f);

		Sphere sph = new Sphere();
		sph.sphereCenter = Vector3.zero;
		sph.sphereRad = 1.0f;
		sph.set_material (mat_ptr);

		Instance sphInst = new Instance(sph);
		sphInst.set_material (mat_ptr);
		add_object (sphInst);

		sphInst.set_identity ();
		sphInst.Scale (40, 40, 1);
		sphInst.Translate (0.0f,0.0f,-60);

		Dielectric mat_ptr1 = new Dielectric ();
		mat_ptr.set_eta_in (0.2f);
		mat_ptr.set_eta_in (0.4f);
		mat_ptr1.set_cf_in (new Color (0.0f, 1.0f, 0.0f, 1));
		mat_ptr1.set_cf_out (new Color (0.75f, 1.0f, 0.0f, 1));
		mat_ptr1.set_ks (0.8f);
		mat_ptr1.set_exp (100.0f);
		mat_ptr1.set_ior (1.52f);

		Rectangle rect1 = new Rectangle(Vector3.zero,new Vector3(1,0,0),new Vector3(0,1,0));
		rect1.set_material (mat_ptr1);

		Instance rect1Inst = new Instance(rect1);
		rect1Inst.set_material (mat_ptr1);
		add_object (rect1Inst);

		rect1Inst.set_identity ();
		rect1Inst.Scale (80, 80, 1);
		rect1Inst.Translate (-40,-40,0.0f);

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
		MeshObject  hitobj = null;
		for (int j = 0; j < num_objects; j++)
		{
			if (objects [j].hit (ref ray,ref t,ref shade)) 
			{
				if (t == Mathf.Infinity && tmin == Mathf.Infinity) 
				{
					shade.hit_an_object	= true;
					hitobj =  objects[j];
					tmin = t;
					shade.material_ptr = objects [j].get_material ();
					shade.hit_point = ray.origin + t * ray.direction;
					normal = shade.normal;
					local_hit_point = shade.local_hit_point;
				} 
				else if (t < tmin) 
				{
					shade.hit_an_object	= true;
					hitobj =  objects[j];
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
			shade.hitMesh = hitobj;
			shade.normal = normal;
			shade.local_hit_point = local_hit_point;
		} 
		return(shade);   
	}
}
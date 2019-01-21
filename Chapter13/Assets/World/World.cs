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

	public Texture2D			mapTexture;

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
		vp = new ViewPlane (texture.width,texture.height,1.0f,256);
		vp.max_depth = 5;
		background_color = Constants.black;

		tracer_ptr = new RayCastTracer (this);

		Jittered jit = new Jittered(256);
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

		SV_Matte mat_ptr = new SV_Matte ();
		mat_ptr.set_ka (0.25f);
		mat_ptr.set_kd (0.5f);
		TextureData textData = new TextureData ();
		textData.LoadTextureData (mapTexture);
		mat_ptr.set_cd(textData);

		Rectangle rect = new Rectangle (Vector3.zero, new Vector3 (80, 0, 0), new Vector3 (0, 70, 0), Vector2.zero, new Vector2 (0, 1),new Vector2(1,0));

		Instance rectInst = new Instance(rect);
		rectInst.set_material (mat_ptr);
		add_object (rectInst);

		rectInst.set_identity ();
		rectInst.Scale (1, 1, 1);

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
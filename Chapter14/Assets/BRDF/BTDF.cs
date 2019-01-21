using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTDF : MonoBehaviour {

	public virtual Color f(ref Shade sr,ref Vector3 wo, ref Vector3 wi)
	{
		return Constants.black;
	}

	public virtual Color sample_f(ref Shade sr, ref Vector3 wo,ref Vector3 wi)
	{
		return Constants.black;
	}

	public virtual bool tir(ref Shade sr)
	{
		return false;
	}

	public virtual Color rho(ref Shade sr, ref Vector3 wo)
	{
		return Constants.black;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexSetter : MonoBehaviour 
{
	public Material mat;

	void OnDrawGizmos()
	{
		if (mat == null)
			return;
		mat.SetVector ("_VectorPos", transform.position);
	}
}

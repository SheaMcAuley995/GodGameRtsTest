using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GeneratePlaneMesh : MonoBehaviour {

    private struct meshNode
    {
        Vector3 pos;
        float height;
        float dirtlevel;
    }

    public float size = 1;
    private MeshFilter filter;


	// Use this for initialization
	void Start () {
        filter = GetComponent<MeshFilter>();

        filter.mesh = GenerateMesh();
	}
	
	void Update () {
		
	}

    Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.SetVertices(new List<Vector3>()
        {
            new Vector3(-size * 0.5f, 0, -size * 0.5f),
            new Vector3(size * 0.5f, 0, -size * 0.5f),
            new Vector3(size * 0.5f, 0, size * 0.5f),
            new Vector3(-size * 0.5f, 0, size * 0.5f)
        });

        mesh.SetTriangles(new List<int>()
        {
            0,3,1,
            1,3,2
        },0);

        mesh.SetNormals(new List<Vector3>()
        {
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        });

        return mesh;
    }



}

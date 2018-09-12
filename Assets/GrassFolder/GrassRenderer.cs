using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassRenderer : MonoBehaviour {

    public Mesh grassMesh;
    public Material material;

    public int seed;
    public Vector2 size;
    [Range(1,1000)]
    public int grassNum;
    public float startHeight = 1000;

	// Use this for initialization
	void Update () {
        Random.InitState(seed);
        List<Matrix4x4> matrices = new List<Matrix4x4>(grassNum);
        for(int i = 0; i < grassNum; ++i)
        {
            Vector3 origin = transform.position;
            origin.y = startHeight;
            origin.x += size.x * Random.Range(-0.5f, 0.5f);
            origin.z += size.y * Random.Range(-0.5f, 0.5f);
            Ray ray = new Ray(origin, Vector3.down);
            RaycastHit hit; 
            if(Physics.Raycast(ray,out hit))
            {
                origin = new Vector3(hit.point.x,hit.point.y + 0.5f, hit.point.z);
                matrices.Add(Matrix4x4.TRS(origin, Quaternion.identity, Vector3.one));
            }
            
        }
        Graphics.DrawMeshInstanced(grassMesh, 0, material, matrices);
	}
}

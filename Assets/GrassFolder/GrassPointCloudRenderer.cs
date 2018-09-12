using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassPointCloudRenderer : MonoBehaviour {

  //  public Mesh grassMesh;
   // public Material material;

    private Mesh mesh;
    public MeshFilter filter;
    public LayerMask canGrowLayer;
    public int seed;
    public Vector2 size;
    [Range(1,60000)]
    public int grassNum = 100;
    public float startHeight = 1000;

    private Vector3 lastPosition;
    private List<Matrix4x4> matrices;

    // Use this for initialization
    void Update () {
        if(lastPosition != this.transform.position)
        {

            Random.InitState(seed);
            //matrices = new List<Matrix4x4>(grassNum);
            List<Vector3> positions = new List<Vector3>(grassNum);
            int[] indices = new int[grassNum];
            List<Color> colors = new List<Color>(grassNum);
            List<Vector3> normals = new List<Vector3>(grassNum);

            for (int i = 0; i < grassNum; ++i)
            {
                Vector3 origin = transform.position;
                origin.y = startHeight;
                origin.x += size.x * Random.Range(-0.5f, 0.5f);
                origin.z += size.y * Random.Range(-0.5f, 0.5f);
                Ray ray = new Ray(origin, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, canGrowLayer, QueryTriggerInteraction.Collide))
                {
                    origin = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
                    //matrices.Add(Matrix4x4.TRS(origin, Quaternion.identity, Vector3.one));

                    positions.Add(origin);
                    indices[i] = i;
                    colors.Add(new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f), 1));
                    normals.Add(hit.normal);
                }
            }
            mesh = new Mesh();
            mesh.SetVertices(positions);
            mesh.SetIndices(indices, MeshTopology.Points,0);
            mesh.SetColors(colors);
            mesh.SetNormals(normals);
            filter.mesh = mesh;

            lastPosition = this.transform.position;
        }
        
        //Graphics.DrawMeshInstanced(grassMesh, 0, material, matrices);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandPropSpawner : MonoBehaviour {

    public List<GameObject> objects;
    public LayerMask spawnLayer;
    public Vector3 area;
    public int seed;

    [Range(1,1000)]
    public int objectDensity;
	// Use this for initialization
	void Start () {
        for(int i = 0; i < objectDensity; ++i)
        {
            spawnObject();
            Debug.Log(i);
        }

	}
	
    public void spawnObject()
    {
        Vector3 pos = transform.position + new Vector3(Random.Range(-area.x / 2, area.x / 2), Random.Range(-area.y / 2, area.y / 2), Random.Range(-area.z / 2, area.z / 2));

        Ray ray = new Ray(pos, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit,area.y*0.5f,spawnLayer,QueryTriggerInteraction.Collide))
        {
            GameObject newProp = Instantiate(objects[Random.Range(0, objects.Count)], new Vector3 (hit.point.x,hit.point.y,hit.point.z), Quaternion.identity);
           // Vector3 newPos = newProp.transform.position;
            //newPos.y += (newProp.GetComponent<BoxCollider>().size.y * 0.5f);
            //newProp.transform.position = newPos;
            newProp.transform.position = new Vector3 (newProp.transform.position.x, newProp.transform.position.y + (newProp.GetComponent<BoxCollider>().size.y * 0.5f), newProp.transform.position.z);
            Debug.DrawLine(transform.position, Vector3.down * (area.y * 0.5f));
        }
        
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, area);
    }
}

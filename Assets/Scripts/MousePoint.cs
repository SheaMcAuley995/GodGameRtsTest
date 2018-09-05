using UnityEngine;

public class MousePoint : MonoBehaviour {

    RaycastHit hit;
    public GameObject cursor;

    private float raycastLength = 1000;

	void Update () {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, raycastLength))
        {
            Debug.Log(hit.collider.name);
            if(hit.collider.name == "TerrainMain")
            {
                if(Input.GetMouseButtonDown(1))
                {
                    GameObject targetObj = Instantiate(cursor, new Vector3(hit.point.x, hit.point.y + 1, hit.point.z), Quaternion.identity);
                }
                
            }

        }


        Debug.DrawRay(ray.origin, ray.direction * raycastLength, Color.red);
	}
}

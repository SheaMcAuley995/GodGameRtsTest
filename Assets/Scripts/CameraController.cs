using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public Vector3 targetPos;
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;
    public float scrollSpeed = 20f;
    public float minY = 20f;
    public float maxY = 140f;

    public LayerMask collisionLayer;

	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            targetPos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness)
        {
            targetPos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            targetPos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness)
        {
            targetPos.x -= panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        targetPos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;



        targetPos.x = Mathf.Clamp(targetPos.x, -panLimit.x, panLimit.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
        targetPos.z = Mathf.Clamp(targetPos.z, -panLimit.y, panLimit.y);

        groundCorrection();
        transform.position = targetPos;
	}


    private void groundCorrection()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -transform.up,out hit, minY))
        {
            Debug.Log(hit.transform.name);
        }
    }
}

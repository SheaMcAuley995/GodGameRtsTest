using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public Vector3 targetPos;
    public float panSpeed = 20f;
    public float correctionSpeed = 0.2f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;
    public float scrollSpeed = 20f;
    public float minY = 20f;
    public float maxY = 140f;


    public LayerMask collisionLayer;

    private void Update()
    {
        groundCorrection();
    }
    // Update is called once per frame
    void LateUpdate () {

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        targetPos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        targetPos.z += Input.GetAxisRaw("Vertical") * panSpeed * Time.deltaTime;
        targetPos.x += Input.GetAxisRaw("Horizontal") * panSpeed * Time.deltaTime;

        targetPos.x = Mathf.Clamp(targetPos.x, -panLimit.x, panLimit.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
        targetPos.z = Mathf.Clamp(targetPos.z, -panLimit.y, panLimit.y);

        transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime * panSpeed), Mathf.Lerp(transform.position.y, targetPos.y, Time.deltaTime * correctionSpeed), Mathf.Lerp(transform.position.z, targetPos.z, Time.deltaTime * panSpeed));
	}



    private void groundCorrection()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -transform.up,out hit, minY - 5,collisionLayer,QueryTriggerInteraction.Collide))
        {
            targetPos.y = minY + hit.point.y;
            Debug.Log(targetPos.y);
        }
    }
}

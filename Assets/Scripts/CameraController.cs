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
    public float heightDampening = 5f;
    public float rotationSpeed = 20f;
    public float minY = 20f;
    public float maxY = 140f;
    private float zoomPos = 0;
    public Vector3 target;

    public LayerMask collisionLayer;

    private void Update()
    {
        Move();
        Rotation();
        HeightCalculation();
    }
    // Update is called once per frame
//    void LateUpdate () {
//
//
//
//        float scroll = Input.GetAxis("Mouse ScrollWheel");
//
//        targetPos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
//
//        targetPos = (Vector3.forward * Input.GetAxisRaw("Vertical")) * panSpeed * Time.deltaTime;
//        targetPos.x += Input.GetAxisRaw("Horizontal") * panSpeed * Time.deltaTime;
//
//        targetPos.x = Mathf.Clamp(targetPos.x, -panLimit.x, panLimit.x);
//        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
//        targetPos.z = Mathf.Clamp(targetPos.z, -panLimit.y, panLimit.y);
//
//        transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime * panSpeed), Mathf.Lerp(transform.position.y, targetPos.y, Time.deltaTime * correctionSpeed), Mathf.Lerp(transform.position.z, targetPos.z, Time.deltaTime * panSpeed));
//	}

    private void Move()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(target);
        }
        else
        {
            Vector3 desiredMove = new Vector3(KeyboardInput.x, 0, KeyboardInput.y);
            desiredMove *= panSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = transform.InverseTransformDirection(desiredMove);

            transform.Translate(desiredMove, Space.Self);
        }

    }


    private void HeightCalculation()
    {

        float ScrollWheel = -Input.GetAxis("Mouse ScrollWheel");


        float distanceToGround = DistanceToGround();
        zoomPos += ScrollWheel * Time.deltaTime * scrollSpeed;

            zoomPos = Mathf.Clamp01(zoomPos);
            //minY = distanceToGround + minY;
            float targetHeight = Mathf.Lerp(minY, maxY, zoomPos);
            float difference = 0;

        if (distanceToGround != targetHeight)
            difference = targetHeight - distanceToGround;
        //if(distanceToGround != targetHeight)
        //  difference = targetHeight - distanceToGround;

        transform.position = Vector3.Lerp(transform.position, 
       new Vector3(transform.position.x, targetHeight + difference, transform.position.z), Time.deltaTime * heightDampening);
    }
    private Vector2 KeyboardInput
    {
        get { return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); }
    }

    //private void groundCorrection()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, -transform.up, out hit, minY - 5, collisionLayer, QueryTriggerInteraction.Collide))
    //    {
    //        targetPos.y = minY + hit.point.y;
    //        Debug.Log(targetPos.y);
    //    }
    //    else
    //    {
    //        if (Input.GetKey(KeyCode.Q))
    //        {
    //            transform.eulerAngles = new Vector2(transform.eulerAngles.x, transform.eulerAngles.y + 2);
    //        }
    //        if (Input.GetKey(KeyCode.E))
    //        {
    //            transform.eulerAngles = new Vector2(transform.eulerAngles.x, transform.eulerAngles.y - 2);
    //        }
    //    }
    //}

    private void Rotation()
    {
        transform.Rotate(Vector3.up, RotationDirection * Time.deltaTime * rotationSpeed, Space.World);

    }

    private int RotationDirection
    {
        get
        {
            bool rotateRight = Input.GetKey(KeyCode.E);
            bool rotateLeft = Input.GetKey(KeyCode.Q);
            if (rotateLeft && rotateRight)
                return 0;
            else if (rotateLeft && !rotateRight)
                return -1;
            else if (!rotateLeft && rotateRight)
                return 1;
            else
                return 0;
        }
    }

    private float DistanceToGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, minY - 5, collisionLayer, QueryTriggerInteraction.Collide))
        {
            Debug.Log((hit.point - transform.position));
         //   minY = hit.point.y + minY;
            return (hit.point - transform.position).y;
        }
        else
        {
         //   minY = 20;
            return 0f;
        }
            


    }
}

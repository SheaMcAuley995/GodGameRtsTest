using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePoint : MonoBehaviour {

    RaycastHit hit;

    private float raycastLength = 1000;

	void Update () {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, raycastLength))
        {
            Debug.Log(hit.collider.name);
        }


        Debug.DrawRay(ray.origin, ray.direction * raycastLength, Color.red);
	}
}

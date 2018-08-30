using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour {

    [SerializeField]
    private LayerMask clickableLayer;

	void Update () {
		
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,Mathf.Infinity, clickableLayer))
            {
                hit.collider.GetComponent<ClickOn>().ClickMe();
            }
        }

	}
}

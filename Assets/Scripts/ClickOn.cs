using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOn : MonoBehaviour {

    [SerializeField]
    private Material red;
    [SerializeField]
    private Material green;

    private MeshRenderer Rend;


	// Use this for initialization
	void Start () {
        Rend = GetComponent<MeshRenderer>();
	}

    public void ClickMe()
    {
        Rend.material = green;
    }
}

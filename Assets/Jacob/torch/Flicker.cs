using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    public float max;
    public float min;
    int x = 0;
    public float intensity;
    float current;

    public Light torch;
	
	void Update ()
    {
		if(x == 0 && current < max)
            current += intensity;
        else if(x == 1 && current > min)
            current -= intensity;

        if (x == 0 && current > max)
            x = 1;
        else if (x == 1 && current < min)
            x = 0;

        torch.intensity = current;
    }
}

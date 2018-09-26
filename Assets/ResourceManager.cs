using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour {

    public static ResourceManager instance = null;

    [HideInInspector] public int amount_Rock;
    [HideInInspector] public int amount_Wood;

    public Text text_Rock;
    public Text text_Wood;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);


    }

    public IEnumerator changeText()
    {
        text_Rock.text = amount_Rock.ToString();
        text_Wood.text = amount_Wood.ToString();
        return null;
    }
}

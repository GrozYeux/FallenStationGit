using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLight : MonoBehaviour
{
    Material default_material = null;
    // Start is called before the first frame update
    void Start()
    {
        default_material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnRayCastExit()
    {
        GetComponent<Renderer>().material = default_material;
    }
}
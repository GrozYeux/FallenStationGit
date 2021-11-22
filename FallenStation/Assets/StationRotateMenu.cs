using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationRotateMenu : MonoBehaviour
{
    public float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.Rotate(new Vector3(0.1f,0,1f) * Time.deltaTime * speed, Space.Self);
    }
}

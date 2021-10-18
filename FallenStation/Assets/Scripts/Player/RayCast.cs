using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    GameObject lastHit;
    RaycastHit hit;
    [SerializeField] Material color = null;

    bool click = false;

    // Update is called once per frame
    private void Update()
    {
        if (!click)
            click = Input.GetMouseButtonDown(0);
    }

    void FixedUpdate()
    {
        var ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.DrawRay(transform.position, transform.forward * 10);
            if ((lastHit != null) && (lastHit != hit.collider.gameObject) && lastHit.CompareTag("collectable"))
            {
                lastHit.GetComponent<HighLight>().OnRayCastExit();
            }
            if (hit.collider.gameObject.CompareTag("collectable"))
            {
                Renderer rend = hit.collider.gameObject.GetComponent<Renderer>();
                rend.material = color;
                if (click)
                {
                    gameObject.GetComponent<Collectables>().AddCard(hit.collider.gameObject.name);
                    Destroy(hit.collider.gameObject);
                }
            }
            lastHit = hit.collider.gameObject;
        }
        else
        {
            if (lastHit != null && lastHit.CompareTag("collectable"))
            {
                lastHit.GetComponent<HighLight>().OnRayCastExit();
                lastHit = null;
            }
        }
        click = false;
    }
}
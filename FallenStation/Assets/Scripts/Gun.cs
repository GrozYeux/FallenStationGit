using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public int gunDamage =1;
    public float range =200f;
    public float force= 100f;
    private Camera cam;
    public float fireRate = 0.25f;
    private float nextFire;
    public LayerMask layer;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            
            nextFire = Time.time + fireRate;
            print(nextFire);
 
            //Crée un vecteur au centre de la vue de la caméra
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
 
             
            // Vérifie si le raycast a touché quelque chose
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, range, layer))
            {
                print("Target");
 
                // Vérifie si la cible a un RigidBody attaché
                if (hit.rigidbody != null)
                {
 
                    //AddForce = Ajoute Force = Pousse le RigidBody avec la force de l'impact
                    hit.rigidbody.AddForce(-hit.normal * force);
 
                    //S'assure que la cible touchée a un composant Cible
                   // if (hit.collider.gameObject.GetComponent<Cible>() != null)
                    //{
                        //Envoie les dommages à la cible
                    //    hit.collider.gameObject.GetComponent<Cible>().GetDamage(gunDamage);
                   // }
                }
            }
        }
    }
}

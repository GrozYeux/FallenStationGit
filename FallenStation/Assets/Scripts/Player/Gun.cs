using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Material color = null;
    public int gunDamage = 1;
    public float range = 200f;
    public float force = 100f;
    public float fireRate = 0.25f;
    private float nextFire;
    public LayerMask layer;
    private bool fire = false;
    GameObject lastHit;
    RaycastHit hit;

    [SerializeField] Camera cam;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!fire)
        {
            fire = Input.GetMouseButtonDown(0); 
            if (fire && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                print("Player shoot");
            }
        }
    }
    void FixedUpdate()
     { 
        //Crée un vecteur au centre de la vue de la caméra
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
           
        // Vérifie si le raycast a touché un des layers : collectable et enemy
        if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, range, layer))
        {
            GameObject objHit = hit.collider.gameObject;

            //GESTION DE L'ATTAQUE
            if (fire && objHit.GetComponent<CharacterStats>() != null) //S'assure que la cible touchée peut être blessée
            {
                CharacterCombat playerCombat = GetComponent<CharacterCombat>();
                Debug.Log(objHit.transform.name + " touched.");

                // Vérifie si la cible a un RigidBody attaché
                if (hit.rigidbody != null)
                {
                    //AddForce = Ajoute Force = Pousse le RigidBody avec la force de l'impact
                    //hit.rigidbody.AddForce(-hit.normal * force); 

                }                
                //Envoie les dommages à la cible
                playerCombat.Attack(objHit.GetComponent<CharacterStats>());
            }

            //GESTION DES OBJETS COLLECTABLES
            if ((lastHit != null) && (lastHit != hit.collider.gameObject) && lastHit.CompareTag("collectable"))
            {
                lastHit.GetComponent<HighLight>().OnRayCastExit();
            }

            if (objHit.CompareTag("collectable"))
            {
                Renderer rend = hit.collider.gameObject.GetComponent<Renderer>();
                rend.material = color;
                if (fire)
                {
                    Collectables.Instance.AddObject(hit.collider.gameObject.name);
                    UIManager.Instance.PrintText(hit.collider.gameObject.name);
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
        fire = false;
    }
    
}

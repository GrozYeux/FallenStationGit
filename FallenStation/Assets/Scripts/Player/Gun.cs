using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    GameObject canvasNote;
    TextManager tm;
    GameObject lastHit;
    RaycastHit hit;

    [SerializeField] Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        canvasNote = GameObject.Find("CanvasNote");
        canvasNote.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!fire)
        {
            fire = Input.GetButtonDown("Fire1");
            if (fire && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                print("Tir");
            }
        }
    }
    void FixedUpdate()
     { 
        //Crée un vecteur au centre de la vue de la caméra
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
           
        // Vérifie si le raycast a touché quelque chose
        if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, range, layer))
        {
            Debug.Log("ok");
            if ((lastHit != null) && (lastHit != hit.collider.gameObject) && lastHit.CompareTag("collectable"))
            {
                lastHit.GetComponent<HighLight>().OnRayCastExit();
            }
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
                //}
            }
            if (hit.collider.gameObject.CompareTag("collectable"))
            {
                Renderer rend = hit.collider.gameObject.GetComponent<Renderer>();
                rend.material = color;
                if (fire)
                {
                    // si le nom de l'objet est un codex
                    if (hit.collider.gameObject.name.Contains("Codex"))
                    {
                        
                        string name = hit.collider.gameObject.name;
                       
                        Collectables.Instance.AddNote(hit.collider.gameObject.name);
                        canvasNote.SetActive(true);
                        UINote.Pause();
                        tm = GameObject.Find("NoteManager").GetComponent<TextManager>();
                        
                        tm.DisplayNote(name);
                    }
                    else
                    {
                        Collectables.Instance.AddObject(hit.collider.gameObject.name);

                    }
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

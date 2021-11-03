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
    GameObject lastHit;
    RaycastHit hit;

    [SerializeField] Camera cam;

    private bool interaction = false;
    public float pickUpDistance = 3.0f;

    GameObject canvasNote;
    TextManager tm;

    // Start is called before the first frame update
    void Start()
    {
        canvasNote = GameObject.Find("CanvasNote");
        canvasNote.SetActive(false);
    }

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

        if (!interaction)
        {
            interaction = Input.GetButtonDown("Interaction");
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
            // Remet la couleur du collectable par défaut si on ne vise plus l'objet..
            if ((lastHit != null) && (lastHit != hit.collider.gameObject) && lastHit.layer == LayerMask.NameToLayer("Collectable"))
            {
                lastHit.GetComponent<HighLight>().OnRayCastExit();
            }
            // ..ou si l'on est trop loin de celui-ci
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Collectable") && hit.distance > pickUpDistance)
            {
                hit.collider.gameObject.GetComponent<HighLight>().OnRayCastExit();
            }
            
            // Vérifie si la cible est un collectable
            if (hit.collider.gameObject.CompareTag("access") || hit.collider.gameObject.CompareTag("codex"))
            {
                // Vérifie que l'on ne soit pas trop éloigné
                if(hit.distance < pickUpDistance)
                {
                    //Change material de l'objets
                    Renderer rend = hit.collider.gameObject.GetComponent<Renderer>();
                    rend.material = color;

                    // Ramasse l'objet si on a utilisé la touche d'interaction
                    if (interaction)
                    {
                        GameObject[] collectables; //tableau dans lequel on mettra les objets a destroy (dont les doublons)
                        if (hit.collider.gameObject.CompareTag("access")) //carte dacces
                        {
                            Collectables.Instance.AddObject(hit.collider.gameObject.name);
                            UITextManager.Instance.PrintText("Item " + hit.collider.gameObject.name + " collecté");
                            collectables = GameObject.FindGameObjectsWithTag("access");
                        }
                        else // note du codex
                        {
                            Collectables.Instance.AddNote(hit.collider.gameObject.name);
                            UITextManager.Instance.PrintText("Nouvelle entrée dans le Codex : " + hit.collider.gameObject.name);
                            collectables = GameObject.FindGameObjectsWithTag("codex");
                            canvasNote.SetActive(true);
                            UINote.Pause();
                            tm = GameObject.Find("NoteManager").GetComponent<TextManager>();
                            tm.DisplayNote(hit.collider.gameObject.name);
                            SaveSystem.SaveCodex(Collectables.Instance);
                        }

                        // Supprime le collectable et les doublons si il y en a
                        foreach(GameObject obj in collectables)
                        {
                            if(obj.name == hit.collider.gameObject.name)
                            {
                                Destroy(obj);
                            }

                        }
                    }
                }
                
            }
            // Vérifie si la cible est une porte
            if (hit.collider.gameObject.CompareTag("door") && hit.distance < pickUpDistance)
            {
                // Tente d'ouvrir la porte si on a utilisé la touche d'interaction
                if (interaction)
                {
                    hit.collider.gameObject.GetComponent<Door>().Open();
                }
            }
            lastHit = hit.collider.gameObject;
        }
        else
        {
            // Si on a rien touché et que l'ancien objet touché était un collectable, remet son material par défaut
            if (lastHit != null && lastHit.layer == LayerMask.NameToLayer("Collectable"))
            {
                lastHit.GetComponent<HighLight>().OnRayCastExit();
                lastHit = null;
            }
        }

        fire = false;
        interaction = false;
    }

}

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
    private int munitions = 30;
    private bool canFire = true;

    GameObject canvasNote;
    TextManager tm;

    // Start is called before the first frame update
    void Start()
    {
        canvasNote = GameObject.Find("CanvasNote");
        canvasNote.SetActive(false);
        Collectables.Instance.AddAmoClip(5);
    }

    void Update()
    {
        if (!fire)
        {
            fire = Input.GetButtonDown("Fire1");
            if (fire && Time.time > nextFire && canFire && munitions > 0)
            {
                munitions -= 1;
                nextFire = Time.time + fireRate;
                print("Tir");
            }
        }

        if (!interaction)
        {
            interaction = Input.GetButtonDown("Interaction");
        }

        if((munitions == 0 || Input.GetButtonDown("Reload")) && Collectables.Instance.HaveAmoClip())
        {
            StartCoroutine(Reload());
        }
    }
    void FixedUpdate()
    {
        //Crée un vecteur au centre de la vue de la caméra
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        // Vérifie si le raycast a touché quelque chose
        if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, range, layer))
        {
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

            // Vérifie si la cible est un collectable
            if (hit.collider.gameObject.CompareTag("access") || hit.collider.gameObject.CompareTag("codex") || hit.collider.gameObject.CompareTag("amoClip"))
            {
                // Vérifie que l'on ne soit pas trop éloigné
                if (hit.distance < pickUpDistance)
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
                        else if (hit.collider.gameObject.CompareTag("amoClip"))
                        {
                            Collectables.Instance.AddAmoClip(1);
                            collectables = GameObject.FindGameObjectsWithTag("amoClip");
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
                        foreach (GameObject obj in collectables)
                        {
                            if (obj.name == hit.collider.gameObject.name)
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
                    if (hit.collider.gameObject.TryGetComponent(out Sas sas))
                    {
                        Sas.Save(this.GetComponentInParent<PlayerMovementScript>(),Collectables.Instance);
                        print(sas.name);
                        if (sas.name == "PorteSas")
                        {
                            Sas.nextLevel();
                        }
                    }
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

    private IEnumerator Reload()
    {
        Debug.Log("Reload");
        canFire = false;
        Collectables.Instance.AddAmoClip(-1);
        munitions = 30;
        yield return new WaitForSeconds(2f);
        canFire = true;
    }
}

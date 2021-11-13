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

    TextManager tm;

    // Start is called before the first frame update
    void Start()
    {
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
                print("Player shoot");
            }
        }

        if (!interaction)
        {
            interaction = Input.GetButtonDown("Interaction");
        }

        if((munitions == 0 || (Input.GetButtonDown("Reload") && munitions != 30)) && Collectables.Instance.HaveAmoClip())
        {
            StartCoroutine(Reload());
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
                //Debug.Log(objHit.transform.name + " touched.");

                // Vérifie si la cible a un RigidBody attaché
                if (hit.rigidbody != null)
                {
                    //AddForce = Ajoute Force = Pousse le RigidBody avec la force de l'impact
                    //hit.rigidbody.AddForce(-hit.normal * force); 
                }
                //Envoie les dommages à la cible
                playerCombat.Attack(objHit.GetComponent<CharacterStats>());

            }

            if (fire && objHit.GetComponent<Boss>() != null) //S'assure que la cible touchée est un boss
            {
                Boss boss = objHit.GetComponent<Boss>();
                //Debug.Log(objHit.transform.name + " touched.");

                //Envoie les dommages à la cible
                boss.TakeDamage(5);

            }
            // Remet la couleur du collectable par défaut si on ne vise plus l'objet..
            if ((lastHit != null) && (lastHit != hit.collider.gameObject) && lastHit.layer == LayerMask.NameToLayer("Collectable"))
            {
                lastHit.GetComponent<HighLight>().OnRayCastExit();
            }
            // ..ou si l'on est trop loin de celui-ci
            if (objHit.layer == LayerMask.NameToLayer("Collectable") && hit.distance > pickUpDistance)
            {
                objHit.GetComponent<HighLight>().OnRayCastExit();
            }
            
            // Vérifie si la cible est un collectable
            if (objHit.CompareTag("access") || objHit.CompareTag("codex") || objHit.CompareTag("amoClip"))
            {
                // Vérifie que l'on ne soit pas trop éloigné
                if (hit.distance < pickUpDistance)
                {
                    //Change material de l'objets
                    Renderer rend = objHit.GetComponent<Renderer>();
                    rend.material = color;

                    // Ramasse l'objet si on a utilisé la touche d'interaction
                    if (interaction)
                    {
                        GameObject[] collectables; //tableau dans lequel on mettra les objets a destroy (dont les doublons)
                        if (objHit.CompareTag("access")) //carte dacces
                        {
                            Collectables.Instance.AddObject(objHit.name);
                            UITextManager.Instance.PrintText("Item " + objHit.name + " collecté");
                            collectables = GameObject.FindGameObjectsWithTag("access");
                        }
                        else if (objHit.CompareTag("amoClip")) //chargeur
                        {
                            Collectables.Instance.AddAmoClip(1);
                            UITextManager.Instance.PrintText("1 chargeur collecté");
                            collectables = GameObject.FindGameObjectsWithTag("amoClip");
                        }
                        else // note du codex
                        {
                            Collectables.Instance.AddNote(objHit.name);
                            UITextManager.Instance.PrintText("Nouvelle entrée dans le Codex : " + objHit.name);
                            collectables = GameObject.FindGameObjectsWithTag("codex");
                            UINote.canvasNote.SetActive(true);
                            UINote.Pause();
                            tm = GameObject.Find("CanvasNote").GetComponent<TextManager>();
                            tm.DisplayNote(objHit.name);
                            SaveSystem.SaveCodex(Collectables.Instance);
                        }

                        // Supprime le collectable et les doublons si il y en a
                        foreach (GameObject obj in collectables)
                        {
                            if(obj.name == objHit.name)
                            {
                                Destroy(obj);
                            }

                        }
                    }
                }

            }

            // Vérifie si la cible est une porte
            if (objHit.CompareTag("door") && hit.distance < pickUpDistance)
            {
                // Tente d'ouvrir la porte si on a utilisé la touche d'interaction
                if (interaction)
                {
                    objHit.GetComponent<Door>().Open();
               
                    if (objHit.TryGetComponent(out Sas sas))
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

             lastHit = objHit;
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
